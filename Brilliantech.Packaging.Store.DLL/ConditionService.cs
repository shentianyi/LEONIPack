using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Utilities.EnumUtil;
using Brilliantech.Framework;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.DLL.Helpers;
using System.Transactions;
using Brilliantech.Framework.Messages;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.Repository.Implement;

namespace Brilliantech.Packaging.Store.DLL
{
    public class ConditionService : IConditionService
    {
        public List<EnumItem> GetEnumItemOptions<T>(T _enum)
        {
            Type type = typeof(T);
            List<EnumItem> ei= EnumUitl.GetEnumItemList(type);
            ei.Insert(0, new EnumItem() { Value = 0, Description = "" });
            return ei;
        }


        public ProcessMsg GenPosition(string positionFormat)
        {
            using (TransactionScope trans = new TransactionScope())
            {
                using (IUnitOfWork unit = MSSqlHelper.DataContext())
                {
                    ProcessMsg msg = new ProcessMsg();
                    try
                    {
                        ITraysRep tr = new TraysRep(unit);
                        string position = string.Empty;
                        int count = tr.GetTrayCountByDay();
                        string[] positionFormats = positionFormat.Split(',');
                        position = string.Format("{0} {1}", string.Format(positionFormats[0], DateTime.Now.Day), string.Format(positionFormats[1], count + 1));
                        msg.result = true;
                        msg.AddMessage(ReturnCode.OK, position);
                    }
                    catch (Exception e)
                    {
                        msg.result = false;
                        msg.AddMessage(ReturnCode.Error, "错误：" + e.Message + "\n请联系程序管理员！");
                    }
                    finally {
                        trans.Dispose();
                    }

                    return msg;
                }
            }
        }
    }
}
