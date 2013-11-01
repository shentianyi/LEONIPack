using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.Data.Enum;
using Brilliantech.Packaging.Store.Data.Repository.Interface;

namespace Brilliantech.Packaging.Store.Data.Repository.Implement
{
    public class SinglePackageRep : ISinglePackageRep
    {
       private PackagingStoreDataDataContext context;

       public SinglePackageRep(IUnitOfWork _unit)
       {
           this.context = _unit as PackagingStoreDataDataContext;
       }

       public SinglePackage GetSingleById(string nr)
       {
          List<SinglePackage> list= context.SinglePackage.Where(s => s.packageID.Equals(nr)).ToList();
          if (list.Count>0)
              return list[0];
          return null;
       }

       public bool Valid(string packageId)
       {
           List<TrayItem> tis = context.TrayItem.Where(t => t.packageId.Equals(packageId) &&(TrayStatus) t.Trays.status != TrayStatus.Cancled).ToList();
           return tis.Count == 0;
       }

       public bool Valid(SinglePackage obj)
       {
           throw new NotImplementedException();
       }
    }
}
