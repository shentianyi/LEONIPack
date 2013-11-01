using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.StoreModel;

namespace Brilliantech.Packaging.Store.Data.Repository.Implement
{
    public class TrayItemRep : ITrayItemRep
    {
        private PackagingStoreDataDataContext context;

        public TrayItemRep(IUnitOfWork _unit)
        {
            this.context = _unit as PackagingStoreDataDataContext;
        }


        public void AddMuti(List<TrayItem> list)
        {
            context.TrayItem.InsertAllOnSubmit(list);
        }

        public TrayItem GetSingleById(string nr)
        {
            return context.TrayItem.SingleOrDefault(ti => ti.trayId.Equals(nr));
        }

        public bool Valid(TrayItem obj)
        {
            throw new NotImplementedException();
        }

        public List<SinglePackage> GetSPByTrayId(string trayId)
        {
            return context.TrayItem.OrderBy(ti=>ti.packageId).Where(ti => ti.trayId.Equals(trayId)).Select(ti => ti.SinglePackage).ToList();
        }


        public bool Valid(string objId)
        {
            throw new NotImplementedException();
        }
    }
}
