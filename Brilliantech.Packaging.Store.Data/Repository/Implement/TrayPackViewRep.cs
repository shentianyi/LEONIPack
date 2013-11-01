using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.Repository.Interface;
using Brilliantech.Packaging.Store.Data.StoreModel;

namespace Brilliantech.Packaging.Store.Data.Repository.Implement
{
    public class TrayPackViewRep : ITrayPackViewRep
    {
        private PackagingStoreDataDataContext context;

        public TrayPackViewRep(IUnitOfWork _unit)
        {
            this.context = _unit as PackagingStoreDataDataContext;
        }


        public List<TrayPackView> GetTPVByTrayIdsGropSumPartNr(List<string> trayIds)
        {
            return (from tp in context.TrayPackView.AsEnumerable()
                    where trayIds.Contains(tp.trayId)
                    group tp by  tp.partNr into tpv
                    select new TrayPackView()
                    {
                        trayId = tpv.FirstOrDefault().trayId,
                        warehouse = tpv.FirstOrDefault().warehouse,
                        position = tpv.FirstOrDefault().position,
                        partNr = tpv.FirstOrDefault().partNr,
                        spsatus = tpv.FirstOrDefault().spsatus,
                        tstatus = tpv.FirstOrDefault().tstatus,
                        createTime = tpv.FirstOrDefault().createTime,
                        customerPartNr = tpv.FirstOrDefault().customerPartNr,
                        capa = tpv.Sum(p => p.capa)
                    }).ToList();
        }

        public StoreModel.TrayPackView GetSingleById(string id)
        {
            throw new NotImplementedException();
        }

        public bool Valid(StoreModel.TrayPackView obj)
        {
            throw new NotImplementedException();
        }

        public bool Valid(string objId)
        {
            throw new NotImplementedException();
        }
    }
}
