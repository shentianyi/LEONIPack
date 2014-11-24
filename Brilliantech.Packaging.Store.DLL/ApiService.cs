using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framework;
using RestSharp;
using System.Net;
using System.ServiceModel.Web;

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
                token = config.Get("Token");
                syncContainerAction = config.Get("PrintDataAction");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SyncContainer()
        {
            try
            {
                var req = new RestRequest(syncContainerAction,Method.GET);
                req.RequestFormat = DataFormat.Json;
                req.AddParameter("code", "p001");
                req.AddParameter("id", "1");

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
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
            if (res.StatusCode != HttpStatusCode.OK)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = res.StatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = res.StatusDescription;
                throw new WebFaultException<string>(res.StatusDescription, res.StatusCode);
            }
            return res;
        }


    }
}
