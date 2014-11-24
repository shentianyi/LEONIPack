 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.DLL.Message;
using Brilliantech.Framework.Messages;
using Brilliantech.Packaging.Store.Data.StoreModel;
using Brilliantech.Packaging.Store.DLL.Helpers;
using System.Collections;
using Brilliantech.ReportGenConnector;
using Brilliantech.Framework;

namespace Brilliantech.Packaging.Store.DLL
{
    public class PrintService : IPrintService
    {
        public ProcessMsg Print(Hashtable printConfig, PrintDataMessage pmsg)
        {
            ProcessMsg msg = new ProcessMsg();
            try
            {
                IReportGen gen = new TecITGener();
                foreach (PrintTask task in pmsg.PrintTask)
                {
                    task.Config.Printer = printConfig["PrinterName"].ToString();
                    task.Config.NumberOfCopies = int.Parse(printConfig["Copy"].ToString());
                    task.Config.Template = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, printConfig["Template"].ToString());
                    gen.Print(task.DataSet, task.Config);
                }
                msg.result = true;
                msg.AddMessage(ReturnCode.OK, "打印成功！");
            }
            catch (Exception e)
            {
                msg.result = false;
                msg.AddMessage(ReturnCode.Fail, "打印错误：" + e.Message);
            }
            return msg;
        }

        public PrintDataMessage GenSingleTrayLabel(string trayId,string dateFormat,string[] keepers)
        {
            PrintDataMessage pdm = new PrintDataMessage();
            ValidateMsg<Trays> msg = TraysHelper.TrayCanPrint(trayId);
            if (msg.Valid)
            {
                List<TrayPackView> tpv = TrayPackViewHelper.GetTPVByTrayIdsGropSumPartNr(new List<string>() { trayId });
                string[] dateFormats = dateFormat.Split(',');
                List<PrintTask> tasks = new List<PrintTask>();
                foreach (string keeper in keepers)
                {
                    RecordSet rs = new RecordSet();
                    PrintTask task = new PrintTask() { DataSet = rs };
                    foreach (TrayPackView t in tpv)
                    {
                        RecordData label = new RecordData();
                        label.Add("TrayId", t.trayId);
                        label.Add("Warehouse", t.warehouse);
                        label.Add("Position", t.position);
                        label.Add("customerPNr", t.customerPartNr);
                        label.Add("PartNr", t.partNr);
                        label.Add("Capa", t.capa.ToString());
                        label.Add("Keeper", keeper);
                        label.Add("CreateTime",t.createTime.ToString(dateFormats[0]));
                        label.Add("StoreCreateTime", t.createTime.ToString(dateFormats[1]));
                        rs.Add(label);
                    }
                    tasks.Add(task);
                }
                pdm.PrintTask = tasks;
                pdm.ReturnedResult = true;
            }
            else
            {
                pdm.ReturnedResult = false;
                pdm.ReturnedMessage.Add(msg.ToString());
            }
            return pdm;
        }
    
    }
}
