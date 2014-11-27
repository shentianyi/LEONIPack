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
        private static string storeContainerAction;
        private static string unstoreContainerAction;

        static ApiService()
        {
            try
            {
                config = new ConfigUtil("API", "api.ini");
                host = config.Get("Host");
                token = config.Get("Token");
                storeContainerAction = config.Get("StoreContainerApi");
                unstoreContainerAction = config.Get("UnStoreContainerApi");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SyncStoreContainer(Hashtable containers)
        {
            try
            {
                var req = new RestRequest(storeContainerAction, Method.POST);
                req.RequestFormat = DataFormat.Json;
                string d = JsonConvert.SerializeObject(containers);
                req.AddParameter("data", d);
                var res = new ApiService().Execute(req);
                Msg<string> msg = parse<Msg<string>>(res.Content);
                if (msg.Result == false)
                {
                    throw new ApiException(msg.Content);
                }
                return msg.Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SyncUnStoreContainer(string id,string whouse)
        {
            try
            {
                var req = new RestRequest(unstoreContainerAction, Method.POST);
                req.RequestFormat = DataFormat.Json;
                req.AddParameter("id", id);
                req.AddParameter("whouse", whouse);
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
            return res;
        }
         

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
