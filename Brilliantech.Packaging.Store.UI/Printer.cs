using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.DLL;
using Brilliantech.Packaging.Store.DLL.Message;
using System.Collections;
using Brilliantech.Framework;
using Brilliantech.Framework.Messages;

namespace Brilliantech.Packaging.Store.UI
{
    public class Printer
    {
        public static Message PrintTrayLabel(string trayId) 
        {
            Message msg=new Message();
            PrintService printService = new PrintService();
            PrintDataMessage pmsg = printService.GenSingleTrayLabel(trayId
                , new ConfigUtil("DATEFORMAT", "config.ini").Get("DATEFORMAT")
                , new ConfigUtil("KEEPER", "config.ini").Get("KEEPER").Split(','));
            if (pmsg.ReturnedResult)
            {
                Hashtable printConfig = PrinterUtil.GetPrinterConfig();;
                ProcessMsg prmsg = printService.Print(printConfig, pmsg);
                msg.Result = prmsg.result;
                msg.Content = prmsg.GetAllLevelMsgs();
            }
            else
            {
                msg.Result = pmsg.ReturnedResult;
                msg.Content = pmsg.GetMsgText();
            }
            return msg;
        }
    }
}
