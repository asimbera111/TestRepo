using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Helpers.Request;
using WebServiceAutomation.Helpers.ResponseData;
using WebServiceAutomation.Model;
using WebServiceAutomation.Model.Json_Model;
using WebServiceAutomation.Model.Xml_Model;

namespace WebServiceAutomation.ParallelExecution
{
    [TestClass]
    public class TestParallelExecution
    {
        private string delayGetUrl = "http://localhost:8080/laptop-bag/webapi/delay/all";
        private string delayGetUrlWithId = "http://localhost:8080/laptop-bag/webapi/delay/find";
        private string delayPostUrl = "http://localhost:8080/laptop-bag/webapi/delay/add";
        private string delayPutUrl = "http://localhost:8080/laptop-bag/webapi/delay/update";

        private  RestResponse restResponse;
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private Random random = new Random();

        private HttpClientHelperAsync httpClientHelperAsync = new HttpClientHelperAsync();


        [TestMethod]
        private void SendGetRequest()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            restResponse = httpClientHelperAsync.PerformGetRequest(delayGetUrl, httpHeader).GetAwaiter().GetResult();
            List<Root> rootObj = ResponseDataHelper.DeserializeJsonResponse<List<Root>>(restResponse.ResponseData);
            Console.WriteLine(rootObj.ToString());
        }

        [TestMethod]
        private void SendPostRequest()
        {
            int id = random.Next(1000);
            string jsonData = "{" +
                            "\"BrandName\": \"Alienware\"," +
                            "\"Features\": {" +
                            "\"Feature\": [" +
                            "\"8th Generation Intel® Core™ i5-8300H\"," +
                            "\"Windows 10 Home 64-bit English\"," +
                            "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
                            "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
                            "]" +
                            "}," +
                            "\"Id\": " + id + "," +
                            "\"LaptopName\": \"Alienware M17\"" +
                            "}";
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", jsonMediaType);
            restResponse = httpClientHelperAsync.PerformPostRequest(delayPostUrl, httpHeader, jsonData, jsonMediaType).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlData = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Console.WriteLine(xmlData.ToString());
        }

        [TestMethod]
        private void SendPutMethod()
        {
            int id = random.Next(1000);
            string xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>()
            {
                { "Accept", xmlMediaType}
            };
            restResponse = httpClientHelperAsync.PerformPostRequest(delayPostUrl, httpHeaders, xmlData, xmlMediaType).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            xmlData = "<Laptop>" +
                                    "<BrandName>Alienware</BrandName>" +
                                    "<Features>" +
                                        "<Feature>8th Generation IntelÂ® Coreâ„¢ i5-8300H</Feature>" +
                                        "<Feature>Windows 10 Home 64-bit English</Feature>" +
                                        "<Feature>NVIDIAÂ® GeForceÂ® GTX 1660 Ti 6GB GDDR6</Feature>" +
                                        "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>" +
                                        "<Feature>1TB of SSD</Feature>" +
                                    "</Features>" +
                                "<Id>" + id + "</Id>" +
                                "<LaptopName>Alienware M17</LaptopName>" +
                             "</Laptop>";

            restResponse = httpClientHelperAsync.PerformPutRequest(delayPutUrl, httpHeaders,xmlData, xmlMediaType).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);
            restResponse = httpClientHelperAsync.PerformGetRequest(delayGetUrlWithId, httpHeaders).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            Laptop xmlObj = ResponseDataHelper.DeserializeXmlResponse<Laptop>(restResponse.ResponseData);
            Assert.IsTrue(xmlObj.Features.Feature.Contains("1TB of SSD"), "Item not found");
        }

        [TestMethod]
        public void TestTask()
        {
            Task get = new Task(() => { SendGetRequest(); });
            get.Start();

            Task post = new Task(() => { SendPostRequest(); });
            post.Start();

            Task put = new Task(() => { SendPutMethod(); });
            put.Start();

            get.Wait();
            post.Wait();
            put.Wait();
        }

        [TestMethod]
        public void TestTaskUsingTaskFactory()
        {
            var getTask = Task.Factory.StartNew(() =>
            {
                SendGetRequest();

            });

            var putTask = Task.Factory.StartNew(() =>
            {
                SendPutMethod();

            });

            var postTask = Task.Factory.StartNew(() =>
            {
                SendPostRequest();

            });

            getTask.Wait();
            putTask.Wait();
            postTask.Wait();
        }
    }
}
