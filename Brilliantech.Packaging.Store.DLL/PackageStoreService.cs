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

namespace Brilliantech.Packaging.Store.DLL
{
    public class PackageStoreService : IPackageStoreService
    {
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
                        msg.Message.Add("包装箱:"+packageId+" 不存在");
                    }
                    else
                        if (!PackageStatusHelper.CanStoredStatus(sp.status))
                        {
                            msg.Valid = false;
                            msg.Message.Add("包装箱:" + packageId + " 还未结束包装或标签为开箱标签！");
                        }
                        else
                            if (!spr.Valid(sp.packageID)) {
                                msg.Valid = false;
                                msg.Message.Add("包装箱:" + packageId + " 已经入库！");
                            }
                }
                catch (Exception e) {
                    msg.Valid = false;
                    msg.Message.Add("错误：" + e.Message);
                }
                return msg;
            }
        }

        public ProcessMsg CompleteStore(List<string> packageId, string whouse, string posi)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                using (IUnitOfWork unit = MSSqlHelper.DataContext())
                {
                    ProcessMsg msg =new ProcessMsg ();
                    try
                    {
                        string trayId ="T"+DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ITraysRep tr = new TraysRep(unit);
                        ITrayItemRep tir = new TrayItemRep(unit);
                        Trays ts = new Trays()
                        {
                            trayId = trayId,
                            createTime = DateTime.Now,
                            warehouse = whouse,
                            position = posi,
                            status =(int)TrayStatus.Stored,
                            rowguid = Guid.NewGuid()
                        };

                        List<TrayItem> tis = new List<TrayItem>();
                        foreach (string pid in packageId)
                            tis.Add(new TrayItem()
                            {
                                itemId = Guid.NewGuid(),
                                trayId = ts.trayId,
                                packageId = pid,
                                rowguid = Guid.NewGuid()
                            });

                        tr.AddSingle(ts);
                        tir.AddMuti(tis);
                        unit.Submit();
                        trans.Complete();
                        msg.result = true;
                        msg.AddMessage(ReturnCode.OK, ts.trayId );
                    }
                    catch (Exception e)
                    {
                        msg.result = false;
                        msg.AddMessage(ReturnCode.Error, "错误：" + e.Message+"\n请联系程序管理员！");
                    }
                    finally {
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
                        foreach (Trays ts in tis)
                            ts.status = (int)TrayStatus.Cancled;
                        unit.Submit();
                        trans.Complete();
                        msg.result = true;
                        msg.AddMessage(ReturnCode.OK, "入库取消成功！");
                    }
                    catch (Exception e)
                    {
                        msg.result = false;
                        msg.AddMessage(ReturnCode.Error,"错误：" + e.Message+"\n请联系程序管理员！");
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
    }
}
