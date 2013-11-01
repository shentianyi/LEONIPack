using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework.Messages;
using Brilliantech.Packaging.Store.Data.StoreModel;
using System.Collections;

namespace Brilliantech.Packaging.Store.DLL
{
    public interface IPackageStoreService
    {
        /// <summary>
        /// validate single package id
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        ValidateMsg<SinglePackage> SingleCheckStore(string packageId);

        /// <summary>
        /// compelete the store
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="warehouse"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        ProcessMsg CompleteStore(List<string> packageId,string warehouse,string position);

        /// <summary>
        /// cancle stored tray
        /// </summary>
        /// <param name="trayId"></param>
        /// <returns></returns>
        ProcessMsg CancleStored(List<string> trayIds);

        /// <summary>
        /// search trays by muti conditions
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        List<Trays> Search(Hashtable conditions);

        /// <summary>
        /// get single packages by tray id
        /// </summary>
        /// <param name="trayId"></param>
        /// <returns></returns>
        List<SinglePackage> TraySPDetail(string trayId);
    }
}
