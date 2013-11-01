using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.StoreModel; 
namespace Brilliantech.Packaging.Store.Data.Repository.Interface
{
    public interface ITrayItemRep : IBaseRep<TrayItem>
    {
       void AddMuti(List<TrayItem> list);
       List<SinglePackage> GetSPByTrayId(string trayId);
    }
}
