using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.StoreModel;
using System.Collections;

namespace Brilliantech.Packaging.Store.Data.Repository.Interface
{
    public interface ITraysRep : IBaseRep<Trays>
    {
        List<Trays> GetByIds(List<string> ids); 
        List<Trays> GetByConditions(Hashtable conditions);
        List<Trays> GetUnsync();
        void AddSingle(Trays tray);
        int GetTrayCountByDay();
    }
}
