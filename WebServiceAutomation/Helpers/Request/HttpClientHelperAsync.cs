using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model;

namespace WebServiceAutomation.Helpers.Request
{
    public class HttpClientHelperAsync
    {
        private static HttpClient httpClient;

        private HttpClient AddHeadersAndCreateHttpClient(Dictionary<string, string> httpHeader)
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

        private HttpRequestMessage CreateHttpRequestMessage(string requestUrl, HttpMethod httpMethod, HttpContent httpContent)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);
            if(!((httpMethod == HttpMethod.Get) || (httpMethod == HttpMethod.Delete)))
                httpRequestMessage.Content = httpContent;
            return httpRequestMessage;
        }
  
        public async Task<RestResponse> PerformGetRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUrl);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statusCode, responseData);
        }

        public async Task<RestResponse> PerformPostRequest(string requestUrl, Dictionary<string, string> httpHeader, string data,
                                                      string mediatype)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediatype);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUrl, httpContent);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statusCode, responseData);
        }

        public async Task<RestResponse> PerformPutRequest(string requestUrl, Dictionary<string, string> httpHeader, string data,
                                                      string mediatype)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpContent httpContent = new StringContent(data, Encoding.UTF8, mediatype);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(requestUrl, httpContent);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statusCode, responseData);
        }
        
        public async Task<RestResponse> PerformDeleteRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(requestUrl);
            int statusCode = (int)httpResponseMessage.StatusCode;
            var responseData = await httpResponseMessage.Content.ReadAsStringAsync();
            return new RestResponse(statusCode, responseData);
        }

    }
}
