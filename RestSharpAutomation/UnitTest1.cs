using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace RestSharpAutomation
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            /**
            1. Create the client
            2. Create the request
            3. Send the request using the client
            4. Capture the response
             */

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest();
        }
    }
}
