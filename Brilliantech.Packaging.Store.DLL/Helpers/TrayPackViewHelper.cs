using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.Repository.Implement;

namespace Brilliantech.Packaging.Store.DLL.Helpers
{
    public class TrayPackViewHelper
    {
        public static List<TrayPackView> GetTPVByTrayIdsGropSumPartNr(List<string> trayIds)
        {
            IUnitOfWork unit = MSSqlHelper.DataContext();
            ITrayPackViewRep tpvr = new TrayPackViewRep(unit);
            return tpvr.GetTPVByTrayIdsGropSumPartNr(trayIds);
        }
    }
}
