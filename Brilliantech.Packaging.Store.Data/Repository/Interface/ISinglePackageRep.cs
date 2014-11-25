using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.Data.Enum; 

namespace Brilliantech.Packaging.Store.Data.Repository.Interface
{
    public interface ISinglePackageRep:IBaseRep<SinglePackage>
    {
        List<SinglePackage> GetListByIds(List<string> ids);
    }
}
