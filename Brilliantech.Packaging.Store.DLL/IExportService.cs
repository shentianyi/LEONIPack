using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Messages;

namespace Brilliantech.Packaging.Store.DLL
{
   public interface IExportService
    {
       ProcessMsg ExportTraySumPartCSV(List<string> trayIds,string filename,List<string> fieldNames);
    }
}
