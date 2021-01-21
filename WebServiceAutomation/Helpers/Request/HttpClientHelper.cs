using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.Helpers.Request
{
    public static class HttpClientHelper
    {
        private static HttpClient httpClient;
        private static HttpRequestMessage httpRequestMessage;
        private static RestResponse restResponse;

        private static HttpClient AddHeadersAndCreateHttpClient(Dictionary<string, string> httpHeader)
        {
            HttpClient httpClient = new HttpClient();
            if (null != httpHeader)
            {
                foreach (string key in httpHeader.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, httpHeader[key]);
                }
            }            
            return httpClient;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);
            if(!((httpMethod == HttpMethod.Get) || (httpMethod == HttpMethod.Delete)))
                httpRequestMessage.Content = httpContent;
            return httpRequestMessage;
        }

        private static RestResponse SendRequest(Dictionary<string, string> httpHeader, string requestUrl, 
                                        HttpMethod httpMethod, HttpContent httpContent)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            httpRequestMessage = CreateHttpRequestMessage(requestUrl, httpMethod, httpContent);

            try
            {
                Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode,
                                                httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
                restResponse = new RestResponse(500, e.Message);
            }
            finally
            {
                // ? - NullCheckOperator. 
                // Before calling the Dispose(),it will check if the object is null or not.if it is null, it wont invoke the Dispose().
                httpClient?.Dispose();
                httpRequestMessage?.Dispose();
            }
            return restResponse;
        }

        public static RestResponse PerformGetRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            return SendRequest(httpHeader, requestUrl, HttpMethod.Get, null);
        }

        public static RestResponse PerformPostRequest(string requestUrl, Dictionary<string, string> httpHeader, HttpContent httpContent)
        {
            return SendRequest(httpHeader, requestUrl, HttpMethod.Post, httpContent);
        }

        public static RestResponse PerformPostRequest(string requestUrl, Dictionary<string, string> httpHeader, string data, 
                                                      string mediatype)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediatype);
            return PerformPostRequest(requestUrl, httpHeader, httpContent);
        }

        public static RestResponse PerformPutRequest(string requestUrl, Dictionary<string, string> httpHeader, string data,
                                                      string mediatype)
        {
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediatype);
            return SendRequest(httpHeader, requestUrl, HttpMethod.Put, httpContent);
        }

        public static RestResponse PerformPutRequest(string requestUrl, Dictionary<string, string> httpHeader, HttpContent content)
        {
           return SendRequest(httpHeader, requestUrl, HttpMethod.Put, content);
        }

        public static RestResponse PerformDeleteRequest(string requestUrl)
        {
            return SendRequest(null, requestUrl, HttpMethod.Delete, null);
        }

        public static RestResponse PerformDeleteRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            return SendRequest(httpHeader, requestUrl, HttpMethod.Delete, null);
        }

    }
}
