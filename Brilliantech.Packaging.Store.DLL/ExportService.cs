using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Messages;
using Brilliantech.Packaging.Store.DLL.Helpers;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Framework;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.DLL
{
    public class ExportService : IExportService
    {
        public ProcessMsg ExportTraySumPartCSV(List<string> trayIds, string filename, List<string> fieldNames)
        {
            ProcessMsg msg = new ProcessMsg() { result = false };
            try
            {
                List<TrayPackView> tpv = TrayPackViewHelper.GetTPVByTrayIdsGropSumPartNr(trayIds);
                // List<TrayPackView> ts = tpv.ToList();
                List<string> updateTrayIds = new List<string>();
                CSVDataSet ds = new CSVDataSet();
                foreach (TrayPackView v in tpv)
                {
                    CSVDataRecord r = new CSVDataRecord();
                    List<string> values = ClassUtil.GetModelValues(fieldNames, v);
                    if (values != null)
                        foreach (string value in values)
                            r.Add(value);
                    //r.Add(v.partNr);
                    //r.Add(v.capa.ToString());
                    if (TrayPackStatusHelper.CanAddPrefix(v.tstatus))
                        r.Add(v.TsStatusCN);

                    ds.Add(r);
                    if (TrayPackStatusHelper.CanUpdateToExported(v.tstatus))
                        updateTrayIds.Add(v.trayId);
                }
                CSVUtil.GenCSVFile(ds, filename);
                if (updateTrayIds.Count > 0)
                    TraysHelper.UpdateTraysStatus(updateTrayIds, TrayStatus.Exported);
                msg.result = true;
                msg.AddMessage(ReturnCode.OK, "成功导出CSV文件");
            }
            catch (Exception e)
            {
                msg.AddMessage(ReturnCode.Error, e.Message);
            }
            return msg;
        }
    }
}