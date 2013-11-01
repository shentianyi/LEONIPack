using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.DLL.Message;
using Brilliantech.ReportGenConnector;
using System.Collections;
using Brilliantech.Framework.Messages;

namespace Brilliantech.Packaging.Store.DLL
{
    public interface IPrintService
    {
        ProcessMsg Print(Hashtable printConfig, PrintDataMessage msg);
        PrintDataMessage GenSingleTrayLabel(string trayId, string dateFormat,string[] keepers);
    }
}
