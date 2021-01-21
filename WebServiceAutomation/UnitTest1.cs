using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebServiceAutomation
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Step 1 - Create http client
            HttpClient httpClient = new HttpClient();

            //Close the http connection
            httpClient.Dispose();
        }
    }
}
