using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.Packaging.Store.Data.StoreModel
{
   public interface IUnitOfWork:IDisposable
    {
        void Submit();
    }
}
