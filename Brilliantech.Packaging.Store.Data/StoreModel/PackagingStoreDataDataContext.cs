using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.Packaging.Store.Data.StoreModel
{
    public partial class PackagingStoreDataDataContext : IUnitOfWork
    {
        public void Submit()
        {
            this.SubmitChanges();
        }
    }
}
