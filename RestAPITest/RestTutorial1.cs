using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace RestAssuredTest
{
    [TestFixture]
    public class RestTutorial1
    {
        [Test]
        public void GetRequestUser()
        {
            RestClient restClient = new RestClient("http://localhost:3000/");
            RestRequest restRequest = new RestRequest("posts/{postid}", Method.GET);
            restRequest.AddUrlSegment("postid", 1);

            IRestResponse restResponse = restClient.Execute(restRequest);
          
           // string response = restResponse.Content;
           

            Assert.That(restResponse.StatusCode,Is.EqualTo(HttpStatusCode.OK));

           
            var deserialize = new JsonDeserializer();

            var output = deserialize.Deserialize<Dictionary<string, string>>(restResponse);
            
            
            var result = output["title"];

            Assert.That(result, Is.EqualTo("Selenium with C"), "name not correct");

        }

        [Test]
        
        public void PostRequestUser()
        {
            RestClient restClient = new RestClient("http://localhost:3000/");
            RestRequest restRequest = new RestRequest("posts/{postid}/profile", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(new { name = "Rajeev" });
                
            restRequest.AddUrlSegment("postid",1);
            IRestResponse restResponse = restClient.Execute(restRequest);

            var deserialize = new JsonDeserializer();

            var output = deserialize.Deserialize<Dictionary<string, string>>(restResponse);


            var result = output["name"];

            Assert.That(result, Is.EqualTo("Rajeev"), "name not correct");
        }
        [Test]

        public void PostRequestPosts()
        {
            RestClient restClient = new RestClient("http://localhost:3000/");
            RestRequest restRequest = new RestRequest("posts", Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(new Posts(){ id = "20",author="sanjay",title="restsharp" });

            var restResponse = restClient.Execute(restRequest);
            
            //deserilization using generic method
            var deserialize = new JsonDeserializer();

            var output = deserialize.Deserialize<Dictionary<string, string>>(restResponse);


            var result = output["author"];

            Assert.That(result, Is.EqualTo("sanjay"), "name not correct");
        }
        [Test]
        public void PostWithGenereicMethod()
        {
            RestClient restClient = new RestClient("http://localhost:3000/");
            RestRequest restRequest = new RestRequest("posts", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            restRequest.AddJsonBody(new Posts() { id = "22", author = "sanjay", title = "restsharp" });

            //deserilization using generic method
            var restResponse = restClient.Execute<Posts>(restRequest);

            
            Assert.That(restResponse.Data.author, Is.EqualTo("sanjay"), "name not correct");
        }

        [Test]
        public void PostWithAsyncMethod()
        {
            RestClient restClient = new RestClient("http://localhost:3000/");
            RestRequest restRequest = new RestRequest("posts", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            restRequest.AddJsonBody(new Posts() { id = "23", author = "sanjay", title = "restsharp" });

            //deserilization using generic method
            //var restResponse = restClient.Execute<Posts>(restRequest);

            var result = ExecuteAsyncRequest<Posts>(restClient, restRequest).GetAwaiter().GetResult();
            Assert.That(result.Data.author, Is.EqualTo("sanjay"), "name not correct");
        }

        private async Task<IRestResponse<T>> ExecuteAsyncRequest<T>(RestClient client,IRestRequest request) where T:class,new()
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();
            client.ExecuteAsync<T>(request, restResponse =>
            {
                if (restResponse.ErrorException != null)
                {
                    const string message = "Error reteriving response";


                }
                taskCompletionSource.SetResult(restResponse);
            });
            return await taskCompletionSource.Task;

        }

    }
}
