using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.WCF.Data;
using Brilliantech.ReportGenConnector;

namespace Brilliantech.Packaging.Store.DLL.Message
{
    public class PrintDataMessage : ServiceMessage
    {
        private List<PrintTask> pf_printTask;
        public List<PrintTask> PrintTask
        {
            get
            {
                if (pf_printTask == null)
                {
                    pf_printTask = new List<PrintTask>();
                }
                return pf_printTask;
            }
            set { pf_printTask = value; }
        }
    }
}
