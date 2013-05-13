using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.Data;
using Brilliantech.Packaging.EpmIntegration.EpmServiceProvider;
using Brilliantech.Packaging.EpmIntegration.Util;

namespace Brilliantech.Packaging.EpmIntegration
{
    public class Integration
    {
        /// <summary>
        /// 讲信息传送入EPM的API
        /// </summary>
        /// <param name="pack">singlePackage对象</param>
        /// <param name="newItem">PackageItem对象</param>
        public void notifyEpm(SinglePackage pack, PackageItem newItem, string productionline)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("entityId", productionline);
                data.Add("packTime", ((long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString());
                data.Add("productNr", newItem.itemUid.ToString());
                data.Add("partId", pack.partNr);
                new EpmService().AddProductPack(data);
            }
            catch (Exception e)
            {
                LogUtil.Logger.Error(e.Message);
            }
        }

        /// <summary>
        /// 在NotifyEMP方法中始终使用此方法以记录接口状态
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="time"></param>
        /// <param name="exceptionStr"></param>
        public void log(string msg, DateTime time, string exceptionStr)
        {


        }
    }
}
