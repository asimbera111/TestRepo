using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpAutomation.DropBoxApi.ListFolderModel;
using System.IO;

namespace RestSharpAutomation.DropBoxApi
{
    [TestClass]
    public class TestDropBoxApi
    {
        private const string ListEndPointUrl = "https://api.dropboxapi.com/2/files/list_folder";
        private const string CreateFolderEndPointUrl = "https://api.dropboxapi.com/2/files/create_folder_v2";
        private const string DownloadFileEndPointUrl = "https://content.dropboxapi.com/2/files/download";
        private const string AccessToken = "O9-upzhYmwEAAAAAAAAAAWgCf2Su6-_n-f0r9HF8BaHLrlVgdFdtycqTqNR8O2sA";

        [TestMethod]
        public void TestListFolder()
        {
            string body = "{\"path\": \"\",\"recursive\": false,\"include_media_info\": false,\"include_deleted\": false,\"include_has_explicit_shared_members\": false,\"include_mounted_folders\": true,\"include_non_downloadable_files\": true}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest() 
            {
                Resource = ListEndPointUrl
            };
            restRequest.AddHeader("Authorization", "Bearer " + AccessToken);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(body);

            IRestResponse<Root> restResponse = restClient.Post<Root>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestCreateFolder()
        {
            string body = "{\"path\": \"/TestFolder\",\"autorename\": true}";

            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = CreateFolderEndPointUrl
            };
            restRequest.AddHeader("Authorization", "Bearer " + AccessToken);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(body);

            IRestResponse<Root> restResponse = restClient.Post<Root>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestDownloadFile()
        {
            string fileLocation = "{\"path\": \"Get Started with Dropbox.pdf\"}";
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = DownloadFileEndPointUrl
            };
            restRequest.AddHeader("Authorization", "Bearer " + AccessToken);
            restRequest.AddHeader("Dropbox-API-Arg", fileLocation);
            restRequest.RequestFormat = DataFormat.Json;

            var dataInByte = restClient.DownloadData(restRequest);
            File.WriteAllBytes("Get Started with Dropbox.pdf", dataInByte);
        }        
    }
}
