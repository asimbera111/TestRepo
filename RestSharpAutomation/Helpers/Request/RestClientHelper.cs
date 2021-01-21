using RestSharp;
using System.Collections.Generic;

namespace RestSharpAutomation.Helpers.Request
{
    public class RestClientHelper
    {
        private IRestClient GetRestClient()
        {
            IRestClient restClient = new RestClient();
            return restClient;            
        }

        private IRestRequest GetRestRequest(string url, Dictionary<string, string> httpHeader, Method method, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = new RestRequest()
            {
                Method = method,
                Resource = url
            };

            if (null != httpHeader)
            {
                foreach (string key in httpHeader.Keys)
                {
                    restRequest.AddHeader(key, httpHeader[key]);
                }
            }

            if (body != null)
            {
                restRequest.RequestFormat = dataFormat;
                switch (dataFormat)
                {
                    case DataFormat.Json:
                        restRequest.AddBody(body);
                        break;
                    case DataFormat.Xml:
                        restRequest.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
                        restRequest.AddParameter("xmlBody", body.GetType().Equals(typeof(string)) ? body : restRequest.XmlSerializer.Serialize(body), ParameterType.RequestBody);
                        break;
                }
                
            }
            return restRequest;
        }

        private IRestResponse SendRequest(IRestRequest restRequest)
        {
            IRestClient restClient = GetRestClient();
            IRestResponse restResponse = restClient.Execute(restRequest);
            return restResponse;
        }

        private IRestResponse<T> SendRequest<T>(IRestRequest restRequest) where T : new()
        {
            IRestClient restClient = GetRestClient();
            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);
            if (restResponse.ContentType.Equals("application/xml"))
            {
                var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
                restResponse.Data = deserializer.Deserialize<T>(restResponse);
            }
            return restResponse;
        }

        public IRestResponse PerformGetRequest(string url, Dictionary<string, string> httpHeader)
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.GET, null, DataFormat.None);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }

        public IRestResponse<T> PerformGetRequest<T>(string url, Dictionary<string, string> httpHeader) where T: new()
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.GET, null, DataFormat.None);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        public IRestResponse PerformPostRequest(string url, Dictionary<string, string> httpHeader, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.POST, body, dataFormat);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }

        public IRestResponse<T> PerformPostRequest<T>(string url, Dictionary<string, string> httpHeader, object body, DataFormat dataFormat) where T: new()
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.POST, body, dataFormat);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        public IRestResponse PerformPutRequest(string url, Dictionary<string, string> httpHeader, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.PUT, body, dataFormat);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }

        public IRestResponse<T> PerformPutRequest<T>(string url, Dictionary<string, string> httpHeader, object body, DataFormat dataFormat) where T : new()
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.PUT, body, dataFormat);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        public IRestResponse PerformDeleteRequest(string url, Dictionary<string, string> httpHeader)
        {
            IRestRequest restRequest = GetRestRequest(url, httpHeader, Method.DELETE, null, DataFormat.None);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }
    }
}
