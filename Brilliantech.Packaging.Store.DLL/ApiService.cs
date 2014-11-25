using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework;
using RestSharp;
using System.Net;
using System.ServiceModel.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using Brilliantech.Packaging.Store.DLL.Message;
using System.Collections;
using Newtonsoft.Json;

namespace Brilliantech.Packaging.Store.DLL
{
    public class ApiService
    {
        private static ConfigUtil config;
        private static string host;
        private static string token;
        private static string syncContainerAction;

        static ApiService()
        {
            try
            {
                config = new ConfigUtil("API", "api.ini");
                host = config.Get("Host");
                token = config.Get("Token");
                syncContainerAction = config.Get("SyncContainerApi");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SyncContainer(Hashtable containers)
        {
            try
            {
                var req = new RestRequest(syncContainerAction,Method.POST);
                req.RequestFormat = DataFormat.Json;
                req.AddParameter("data", JsonConvert.SerializeObject(containers));
                var res = new ApiService().Execute(req);
                Msg<string> msg = parse<Msg<string>>(res.Content);
                return msg.Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public T Execute<T>(IRestRequest request) where T : new()
        {
            var response = genClient().Execute<T>(request);
            return response.Data;
        }

        public IRestResponse Execute(IRestRequest request)
        {
            var response = genClient().Execute(request);
            return responseHandler(response);
        }

        private RestClient genClient()
        {
            var client = new RestClient();
            client.BaseUrl = host;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
            return client;
        }

        private IRestResponse responseHandler(IRestResponse res)
        {
            //if (res.StatusCode != HttpStatusCode.OK)
            //{
            //    //WebOperationContext.Current.OutgoingResponse.StatusCode = res.StatusCode;
            //    //WebOperationContext.Current.OutgoingResponse.StatusDescription = res.StatusDescription;
            //    throw new WebFaultException<string>(res.StatusDescription, res.StatusCode);
            //}
            return res;
        }

        //private bool setHead()
        //{
        //    WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        //    if (WebOperationContext.Current.IncomingRequest.Method == "OPTIONS")
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers
        //            .Add("Access-Control-Allow-Methods", "POST, OPTIONS, GET");
        //        WebOperationContext.Current.OutgoingResponse.Headers
        //            .Add("Access-Control-Allow-Headers",
        //                 "Content-Type, Accept, Authorization, x-requested-with");
        //        return false;
        //    }
        //    return true;
        //}

        public T parse<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static string stringify(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
