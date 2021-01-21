using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebServiceAutomation.Helpers.Authentication;
using WebServiceAutomation.Helpers.Request;
using WebServiceAutomation.Helpers.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.Json_Model;
using WebServiceAutomation.Model.Xml_Model;

namespace WebServiceAutomation.GetEndPoint
{
    [TestClass]
    public class TestGetEndpoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";
        private string delayGetUrl = "http://localhost:8080/laptop-bag/webapi/delay/all";

        [TestMethod]
        public void TestGetAllEndpoint()
        {
            //Step1 - Create http client
            HttpClient httpClient = new HttpClient();

            //Step2 - Create and execute the request
            httpClient.GetAsync(getUrl);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndpointWithUri()
        {
            //Step1 - Create http client
            HttpClient httpClient = new HttpClient();

            //Step2 - Create and execute the request
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code => " + (int)statusCode);

            //Response data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData= responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void GetAllEndpointsWithInvalidUrl()
        {
            //Step1 - Create http client
            HttpClient httpClient = new HttpClient();

            //Step2 - Create and execute the request
            Uri getUri = new Uri(getUrl + "/random");
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //status code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code => " + (int)statusCode);

            //Response data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndpointInJsonFormat()
        {
            //Step1 - Create http client
            Uri getUri = new Uri(getUrl);
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/json");

            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code => " + (int)statusCode);

            //Response data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndpointInXmlFormat()
        {
            //Step1 - Create http client
            Uri getUri = new Uri(getUrl);
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Add("Accept", "application/xml");

            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code => " + (int)statusCode);

            //Response data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndpointInJsonFormatUsingAcceptHeader()
        {
            MediaTypeWithQualityHeaderValue jsonHeader = new MediaTypeWithQualityHeaderValue("application/json");

            //Step1 - Create http client
            Uri getUri = new Uri(getUrl);
            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
            requestHeaders.Accept.Add(jsonHeader);
            
            Task<HttpResponseMessage> httpResponse = httpClient.GetAsync(getUri);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code => " + (int)statusCode);

            //Response data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndpointUsingSendAsync()
        {
            Uri getUri = new Uri(getUrl);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(getUrl);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Add("Accept", "application/json");

            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);            
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            Console.WriteLine(httpResponseMessage.ToString());

            //Status Code
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            Console.WriteLine("Status Code => " + statusCode);
            Console.WriteLine("Status Code => " + (int)statusCode);

            //Response data
            HttpContent responseContent = httpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Console.WriteLine(data);

            //Step3 - Close the connection
            httpClient.Dispose();
        }

        // Using statement will automatically call the dispose method to release all resources acquired by http
        // Using statement can only be used for the class that implements the IDisposable interface
        [TestMethod]
        public void TestUsingStatement() 
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");
                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        Console.WriteLine(httpResponseMessage.ToString());

                        //Status Code
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;                       

                        //Response data
                        HttpContent responseContent = httpResponseMessage.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();                        

                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        Console.WriteLine(restResponse.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void TestDeserializationOfJsonResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");
                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        Console.WriteLine(httpResponseMessage.ToString());

                        //Status Code
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;

                        //Response data
                        HttpContent responseContent = httpResponseMessage.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();

                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);
                        List<Root> root = JsonConvert.DeserializeObject<List<Root>>(restResponse.ResponseData);
                        Console.WriteLine(root[0].ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void TestDeserializationOfXmlResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(getUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/xml");
                    Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);

                    using (HttpResponseMessage httpResponseMessage = httpResponse.Result)
                    {
                        Console.WriteLine(httpResponseMessage.ToString());

                        //Status Code
                        HttpStatusCode statusCode = httpResponseMessage.StatusCode;

                        //Response data
                        HttpContent responseContent = httpResponseMessage.Content;
                        Task<string> responseData = responseContent.ReadAsStringAsync();

                        RestResponse restResponse = new RestResponse((int)statusCode, responseData.Result);

                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetailss));
                        TextReader textReader = new StringReader(restResponse.ResponseData);
                        LaptopDetailss xmlData = (LaptopDetailss)xmlSerializer.Deserialize(textReader);
                        Console.WriteLine(xmlData.ToString());

                        // 1st checkpoint(Assertion) for status code
                        Assert.AreEqual(200,restResponse.StatusCode);

                        // 2nd checkpoint(Assertion) for response data
                        Assert.IsNotNull(restResponse.ResponseData);

                        // 3rd checkpoint(Assertion)
                        Assert.IsTrue(xmlData.Laptop.Features.Feature.Contains("Windows 10 Home 64-bit English"), "Item not present");
                    }
                }
            }
        }

        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            RestResponse restResponse = HttpClientHelper.PerformGetRequest(getUrl, httpHeader);
            List<Root> rootObj =  ResponseDataHelper.DeserializeJsonResponse<List<Root>>(restResponse.ResponseData);
            Console.WriteLine(rootObj.ToString());
        }

        [TestMethod]
        public void TestSecureGetEndPoint()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            //httpHeader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==");
            string authHeader = "Basic " + Base64StringConverter.GetBase64String("admin", "welcome");
            httpHeader.Add("Authorization", authHeader);
            RestResponse restResponse = HttpClientHelper.PerformGetRequest(secureGetUrl, httpHeader);
            Assert.AreEqual(200, restResponse.StatusCode);
            List<Root> rootObj = ResponseDataHelper.DeserializeJsonResponse<List<Root>>(restResponse.ResponseData);
            Console.WriteLine(rootObj.ToString());
        }

        [TestMethod]
        public void TestGetEndPoint_Sync()
        {
            //All below request takes 15 secs each to execute - Synchronous call
            HttpClientHelper.PerformGetRequest(delayGetUrl, null);
            HttpClientHelper.PerformGetRequest(delayGetUrl, null);
            HttpClientHelper.PerformGetRequest(delayGetUrl, null);
            HttpClientHelper.PerformGetRequest(delayGetUrl, null);
        }

        [TestMethod]
        public void TestGetEndPoint_Async()
        {
            Task t1 = new Task(GetEndPoint());
            t1.Start();
            Task t2 = new Task(GetEndPoint());
            t2.Start();
            Task t3 = new Task(GetEndPointFail());
            t3.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();
        }

        private Action GetEndPoint()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                { "Accept","Application/json"}
            };
            return new Action(() =>
            {
                RestResponse restResponse =  HttpClientHelper.PerformGetRequest(delayGetUrl, httpHeader);
                Assert.AreEqual(200, restResponse.StatusCode);
            });
        }

        private Action GetEndPointFail()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>()
            {
                { "Accept","Application/json"}
            };
            return new Action(() =>
            {
                RestResponse restResponse = HttpClientHelper.PerformGetRequest(delayGetUrl, httpHeader);
                Assert.AreEqual(201, restResponse.StatusCode);
            });
        }
    }
}
