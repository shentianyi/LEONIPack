using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Messages;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.Data.Repository.Implement;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.Enum;
using System.Transactions;
using Brilliantech.Packaging.Store.DLL.Helpers;
using System.Collections;
using Brilliantech.Framework;
using Brilliantech.Packaging.Store.DLL.Message;
using System.IO;

namespace Brilliantech.Packaging.Store.DLL
{
    public class PackageStoreService : IPackageStoreService
    {
        ConfigUtil config = new ConfigUtil("STORE", "config.ini");

        public ValidateMsg<SinglePackage> SingleCheckStore(string packageId)
        {
            using (IUnitOfWork unit = MSSqlHelper.DataContext())
            {
                ValidateMsg<SinglePackage> msg = new ValidateMsg<SinglePackage>() { Valid = true };
                try
                {
                    ISinglePackageRep spr = new SinglePackageRep(unit);
                    SinglePackage sp = spr.GetSingleById(packageId);
                    msg.Target = sp;
                    if (sp == null)
                    {
                        msg.Valid = false;
                        msg.Message.Add("包装箱:" + packageId + " 不存在");
                    }
                    else
                        if (!PackageStatusHelper.CanStoredStatus(sp.status))
                        {
                            msg.Valid = false;
                            msg.Message.Add("包装箱:" + packageId + " 还未结束包装或标签为开箱标签！");
                        }
                        else
                            if (!spr.Valid(sp.packageID))
                            {
                                msg.Valid = false;
                                msg.Message.Add("包装箱:" + packageId + " 已经入库！");
                            }
                }
                catch (Exception e)
                {
                    msg.Valid = false;
                    msg.Message.Add("错误：" + e.Message);
                }
                return msg;
            }
        }

        public ProcessMsg CompleteStore(List<string> packageIds, string whouse, string posi)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                using (IUnitOfWork unit = MSSqlHelper.DataContext())
                {
                    ProcessMsg msg = new ProcessMsg();
                    try
                    {
                        string trayId = "T" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                        ITraysRep tr = new TraysRep(unit);
                        ITrayItemRep tir = new TrayItemRep(unit);
                        ISinglePackageRep spr = new SinglePackageRep(unit);

                        Trays ts = new Trays()
                        {
                            trayId = trayId,
                            createTime = DateTime.Now,
                            warehouse = whouse,
                            position = posi,
                            status = (int)TrayStatus.Stored,
                            rowguid = Guid.NewGuid()
                        };

                        List<TrayItem> tis = new List<TrayItem>();
                        foreach (string pid in packageIds)
                        {
                            tis.Add(new TrayItem()
                            {
                                itemId = Guid.NewGuid(),
                                trayId = ts.trayId,
                                packageId = pid,
                                rowguid = Guid.NewGuid()
                            });
                        }
                        bool synced = false;
                        // sync container data
                        try
                        {
                            List<SinglePackage> singlePackages = spr.GetListByIds(packageIds);
                            synced = new ApiService().SyncStoreContainer(GenContainers(ts, singlePackages, GetWhouse()));
                            
                        }
                        catch (ApiException ae)
                        {
                            msg.AddMessage(ReturnCode.Warning, ae.Message);
                            synced = false;
                        }
                        catch {
                            synced = false;
                        }
                        ts.sync = synced;

                        tr.AddSingle(ts);
                        tir.AddMuti(tis);
                        unit.Submit();
                        trans.Complete();
                        msg.result = true;
                        if (synced)
                        {
                            msg.AddMessage(ReturnCode.OK, ts.trayId);
                        }
                        else
                        {
                            msg.AddMessage(ReturnCode.OK, ts.trayId);
                            msg.AddMessage(ReturnCode.Warning, "托盘生成成功，但WMS同步失败，请稍候重新同步！");
                        }
                    }
                    catch (Exception e)
                    {
                        msg.result = false;
                        msg.AddMessage(ReturnCode.Error, "错误：" + e.Message + "\n请联系程序管理员！");
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                    return msg;
                }
            }
        }

        public ProcessMsg SyncStore()
        {
            using (TransactionScope trans = new TransactionScope())
            {
                using (IUnitOfWork unit = MSSqlHelper.DataContext())
                {
                    ProcessMsg msg = new ProcessMsg() { result = true };
                    try
                    { 
                        ITraysRep tr = new TraysRep(unit);
                        List<Trays> tis = tr.GetUnsync();
                        ITrayItemRep tir = new TrayItemRep(unit);
                        bool all_synced = true;

                        string error_log = DateTime.Now.ToString("yyyyMMddHHmmsss")+".txt";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"ErrorLog", error_log);
                        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                foreach (Trays ts in tis)
                                {
                                    bool synced = false;
                                    try
                                    {
                                        if (ts.status == (int)TrayStatus.Cancled)
                                        {
                                            synced = new ApiService().SyncUnStoreContainer(ts.trayId, GetWhouse());
                                        }
                                        else
                                        {
                                            List<SinglePackage> singlePackages = tir.GetSPByTrayId(ts.trayId);
                                            synced = new ApiService().SyncStoreContainer(GenContainers(ts, singlePackages, GetWhouse()));
                                        }
                                    }
                                    catch (ApiException ae)
                                    {
                                        sw.WriteLine(ae.Message);
                                        synced = false;
                                    }
                                    catch
                                    {
                                        synced = false;
                                    }
                                    ts.sync = synced;
                                    if (synced == false) { all_synced = false; }
                                }
                            }
                        }
                        unit.Submit();
                        trans.Complete();
                        msg.result = all_synced;
                        if (all_synced)
                        {
                            msg.AddMessage(ReturnCode.OK, "WMS同步成功！");
                        }
                        else
                        {
                            msg.AddMessage(ReturnCode.Warning, "WMS同步失败，查看错误日志:"+error_log+"请稍候重新同步！\n或联系程序管理员！");
                        }
                    }
                    catch (Exception e)
                    {
                        msg.result = false;
                        msg.AddMessage(ReturnCode.Error, "错误：" + e.Message + "\n请联系程序管理员！");
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                    return msg; 
                }
            }
        }

        public ProcessMsg CancleStored(List<string> trayIds)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                using (IUnitOfWork unit = MSSqlHelper.DataContext())
                {
                    ProcessMsg msg = new ProcessMsg() { result = true };
                    try
                    {
                        ITraysRep tr = new TraysRep(unit);
                        List<Trays> tis = tr.GetByIds(trayIds);
                        bool all_synced = true;
                        foreach (Trays ts in tis)
                        {
                            bool synced = false;
                            try
                            {
                                synced = new ApiService().SyncUnStoreContainer(ts.trayId, config.Get("WAREHOUSE"));
                            }
                            catch
                            {
                                all_synced = false;
                            }

                            if (synced == false) { all_synced = false; }
                            ts.sync = synced;
                            ts.status = (int)TrayStatus.Cancled;
                        }
                        unit.Submit();
                        trans.Complete();
                        if (all_synced)
                        {
                            msg.AddMessage(ReturnCode.OK, "入库取消成功！");
                        }
                        else
                        {
                            msg.AddMessage(ReturnCode.OK, "入库取消成功！");
                            msg.AddMessage(ReturnCode.Warning, "入库取消成功，但WMS同步失败，请稍候重新同步！");
                        }
                        msg.result = true;
                    }
                    catch (Exception e)
                    {
                        msg.result = false;
                        msg.AddMessage(ReturnCode.Error, "错误：" + e.Message + "\n请联系程序管理员！");
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                    return msg;
                }
            }
        }

        public List<Trays> Search(System.Collections.Hashtable conditions)
        {
            using (IUnitOfWork unit = MSSqlHelper.DataContext())
            {
                try
                {
                    ITraysRep tr = new TraysRep(unit);
                    return tr.GetByConditions(conditions);
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<SinglePackage> TraySPDetail(string trayId)
        {
            using (IUnitOfWork unit = MSSqlHelper.DataContext())
            {
                try
                {
                    ITraysRep tr = new TraysRep(unit);
                    Trays t = tr.GetSingleById(trayId);
                    if (t == null)
                        return null;
                    ITrayItemRep tir = new TrayItemRep(unit);
                    return tir.GetSPByTrayId(trayId);
                }
                catch
                {
                    return null;
                }
            }
        }

        private Hashtable GenContainers(Trays tray, List<SinglePackage> singlePackages,string whouse)
        {
            Hashtable containers = new Hashtable();
            containers.Add("id", tray.trayId);
            containers.Add("whouse", whouse);
            List<Dictionary<string, string>> packages = new List<Dictionary<string, string>>();

            foreach (var package in singlePackages)
            {
                Dictionary<string, string> p = new Dictionary<string, string>();
                p.Add("id", package.packageID);
                p.Add("part_id", package.partNr);
                p.Add("quantity", package.capa.ToString());
                p.Add("fifo_time", tray.createTime.ToString());
                p.Add("project", package.WorkStation.ProdLine.projectID);
                packages.Add(p);
            }
            containers.Add("packages", packages);
            return containers;
        }

        private string GetWhouse()
        {
            return config.Get("WAREHOUSE");
        }
    }
}
