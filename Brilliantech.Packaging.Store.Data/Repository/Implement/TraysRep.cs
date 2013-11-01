using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.StoreModel;
using System.Collections;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.Data.Repository.Implement
{
    public class TraysRep : ITraysRep
    {
        private PackagingStoreDataDataContext context;

        public TraysRep(IUnitOfWork _unit)
        {
            this.context = _unit as PackagingStoreDataDataContext;
        }


        public void AddSingle(Trays tray)
        {
            context.Trays.InsertOnSubmit(tray);
        }

        public Trays GetSingleById(string nr)
        {
            return context.Trays.SingleOrDefault(t => t.trayId.Equals(nr));
        }


        public bool Valid(StoreModel.Trays obj)
        {
            throw new NotImplementedException();
        }


        public List<Trays> GetByIds(List<string> ids)
        {
            return context.Trays.AsEnumerable().Where(t => ids.Contains(t.trayId)).ToList();
        }


        public List<Trays> GetByConditions(Hashtable c)
        {
            return context.Trays.Where(t =>
                (c["trayId"].ToString().Length == 0 ? true : t.trayId.Equals(c["trayId"].ToString()))
               && (c["startDate"].ToString().Length == 0 ? true : t.createTime >= DateTime.Parse(c["startDate"].ToString()))
               && (c["endDate"].ToString().Length == 0 ? true : t.createTime <= DateTime.Parse(c["endDate"].ToString()))
               && (c["wh"].ToString().Length == 0 ? true : t.warehouse.Equals(c["wh"].ToString()))
               && (c["posi"].ToString().Length == 0 ? true : t.position.Equals(c["posi"].ToString()))
               && (int.Parse(c["status"].ToString()) == 0 ? true : t.status.Equals((TrayStatus)c["status"]))
               && (c["pid"].ToString().Length == 0 ? true : t.TrayItem.Where(ti => ti.packageId.Equals(c["pid"].ToString())).ToList().Count > 0)
               && (c["pnr"].ToString().Length == 0 ? true : t.TrayItem.Where(ti => ti.SinglePackage.partNr.Equals(c["pnr"].ToString())).ToList().Count > 0)
                ).ToList();
        }

        public bool Valid(string objId)
        {
            throw new NotImplementedException();
        }


        public int GetTrayCountByDay()
        {
           DateTime now=DateTime.Now;
           return context.Trays.Count(t => (t.createTime.Year == now.Year
               && t.createTime.Month == now.Month
               && t.createTime.Day == now.Day));
        }
    }
}