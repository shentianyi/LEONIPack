using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Packaging.EpmIntegration.Util;
using Thrift;
using Thrift.Transport;
using Thrift.Protocol;

namespace Brilliantech.Packaging.EpmIntegration
{
    public class Conf
    {
        private static string tServiceHost = "localhost";
        private static int tServicePort = 9001;
        private static int tMaxActive = 50;
        private static int tMaxIdel = 10;
        private static int tMinIdel = 10;
        private static int tMaxWait = 10;
        private static bool tValidateOnGet = true;
        private static bool tValidateOnReturn = true;
        private static bool tValidateWhiledIdle = true;
        private static string tServiceAccessKey;
        static Conf()
        {
            ConfigUtil conf = new ConfigUtil("DatahouseConfig", "conf.ini");
            tServiceHost = conf.Get("Host");
            int.TryParse(conf.Get("Port"), out tServicePort);
            int.TryParse(conf.Get("MaxActive"), out tMaxActive);
            int.TryParse(conf.Get("MaxIdel"), out tMaxIdel);
            int.TryParse(conf.Get("MinIdel"), out tMinIdel);
            int.TryParse(conf.Get("MaxWait"), out tMaxWait);
            bool.TryParse(conf.Get("ValidateOnGet"), out tValidateOnGet);
            bool.TryParse(conf.Get("ValidateOnReturn"), out tValidateOnReturn);
            bool.TryParse(conf.Get("ValidateWhiledIdle"), out tValidateWhiledIdle);
            tServiceAccessKey = conf.Get("AccessKey");
        }
        public static string TSeriviceHost
        {
            get { return Conf.tServiceHost; }
        }

        public static int TServicePort
        {
            get { return Conf.tServicePort; }
        }
        public static int TMaxActive
        {
            get { return Conf.tMaxActive; } 
        }
        public static int TMaxWait
        {
            get { return Conf.tMaxWait; } 
        }
        public static int TMaxIdel
        {
            get { return Conf.tMaxIdel; } 
        }
        public static int TMinIdel
        {
            get { return Conf.tMinIdel; } 
        }
        public static bool TValidateOnGet
        {
            get { return Conf.tValidateOnGet; } 
        }
        public static bool TValidateOnReturn
        {
            get { return Conf.tValidateOnReturn; } 
        }
        public static bool TValidateWhiledIdle
        {
            get { return Conf.tValidateWhiledIdle; } 
        }
        public static string TServiceAccessKey
        {
            get { return Conf.tServiceAccessKey; }
        }
    }
}
