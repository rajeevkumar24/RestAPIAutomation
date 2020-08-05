using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RestAssuredTest
{
    [TestFixture]
    public class RestSharpTutorial2
    {
        private string postUrl = "http://localhost:3000/posts";
        [Test]
        public void TestWithEndPoint()
        {
            string jsondata = "{" +
                                "\"id\": 36," +
                                "\"title\": \"Java\"," +
                                "\"author\": \"JohnF\"" +
                                    "}";

            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest()
            {
                Resource = postUrl
             
            };

            restRequest.AddHeader("Content-Type","application/json");
            restRequest.AddHeader("Accept", "application/xml");

            restRequest.AddJsonBody(jsondata);
            var restResponse = restClient.Post(restRequest);
              Assert.AreEqual(201, (int)restResponse.StatusCode);
            Console.WriteLine(restResponse.Content);


        }
        [Test]
        public void PutRequestPosts()
        {
            RestClient restClient = new RestClient("http://localhost:3000/");
            RestRequest restRequest = new RestRequest("posts/30", Method.PUT);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(new Posts() { author = "kumar", title = "restsharp" });

            var restResponse = restClient.Execute(restRequest);

            //deserilization using generic method
            var deserialize = new JsonDeserializer();

            var output = deserialize.Deserialize<Dictionary<string, string>>(restResponse);


            var result = output["author"];

            Assert.That(result, Is.EqualTo("kumar"), "name not correct");


        }
    }
}
