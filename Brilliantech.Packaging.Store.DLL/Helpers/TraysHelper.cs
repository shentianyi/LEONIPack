using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.Repository.Implement;
using Brilliantech.Framework.Messages;
using Brilliantech.Packaging.Store.Data.Enum;
using Brilliantech.Packaging.Store.DLL.Helpers;

namespace Brilliantech.Packaging.Store.DLL
{
    public class TraysHelper
    {
        public static ValidateMsg<Trays> TrayCanPrint(string trayId)
        {
            using (IUnitOfWork unit = MSSqlHelper.DataContext())
            {
                ValidateMsg<Trays> msg = new ValidateMsg<Trays>() { Valid = false };
                ITraysRep tr = new TraysRep(unit);
                Trays t = tr.GetSingleById(trayId);
                if (t == null)
                    msg.Message.Add("托盘标签："+trayId+" 错误！");
                else
                    msg.Valid = true;
                return msg;
            }
        }

        public static void UpdateTraysStatus(List<string> trayIds,TrayStatus status)
        {
            using (IUnitOfWork unit = MSSqlHelper.DataContext())
            {
                ValidateMsg<Trays> msg = new ValidateMsg<Trays>() { Valid = false };
                ITraysRep tr = new TraysRep(unit);
                List<Trays> ts = tr.GetByIds(trayIds);
                foreach (Trays t in ts)
                    t.status = (int)status;
                unit.Submit();
            }
        }


    }
}
