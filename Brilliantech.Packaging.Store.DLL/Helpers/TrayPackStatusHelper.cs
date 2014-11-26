using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.DLL.Helpers
{
    public class TrayPackStatusHelper
    {
        private static List<TrayStatus> add_prefix_status = null;
        private static List<TrayStatus> update_to_exported_status = null;
        private static List<int> tray_sync_status = null;

        static TrayPackStatusHelper()
        {
            if (add_prefix_status == null)
            {
                add_prefix_status = new List<TrayStatus>(){
                        TrayStatus.Cancled};
            }
            if (update_to_exported_status == null)
                update_to_exported_status = new List<TrayStatus>(){
                    TrayStatus.Stored};
            if (tray_sync_status == null)
                tray_sync_status = new List<int>() { 
                 (int)TrayStatus.Exported,
                 (int)TrayStatus.Stored
                };
        }

        public static bool CanAddPrefix(int s)
        {
            return add_prefix_status.Contains((TrayStatus)s);
        }

        public static bool CanUpdateToExported(int s)
        {
            return update_to_exported_status.Contains((TrayStatus)s);
        }


        public static List<int> TraySyncStatus
        {
            get { return TrayPackStatusHelper.tray_sync_status; }
        }
    }
}
