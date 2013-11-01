using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Utilities.EnumUtil;
using Brilliantech.Packaging.Store.Data.Enum;

namespace Brilliantech.Packaging.Store.DLL.Helpers
{
    public class PackageStatusHelper
    {
        private static List<PackageStatus> status = null;

        static PackageStatusHelper()
        {
            if (status == null)
            {
                status = new List<PackageStatus>(){
                        PackageStatus.Close, 
                        PackageStatus.CloseUnfull,
                        PackageStatus.CloseWithException,
                        PackageStatus.ReworkClose,
                        PackageStatus.ReworkCloseWithException };
            }
        }
        /// <summary>
        /// get allowed list
        /// </summary>
        /// <returns></returns>
        public static List<PackageStatus> AllowedList()
        {
            return status;
        }

        /// <summary>
        /// check contains allowed status
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CanStoredStatus(byte s)
        {
            return status.Contains((PackageStatus)s);
        }
    }
}
