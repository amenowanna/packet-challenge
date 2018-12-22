using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace PacketChallenge
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string apiKey = "";

        static void Main(string[] args)
        {
            apiKey = args[0];
            var device = CreateDevice();
        }

        private static async Task<object> CreateDevice()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);

            var body = @"{
                            ""facility"": ""any"",
                            ""plan"": ""e69c0169-4726-46ea-98f1-939c9e8a3607"",
                            ""hostname"": ""jlopez-challenge"",
                            ""description"": ""JLO's challenge test"",
                            ""operating_system"": ""7014aac5-b97c-4770-8a59-001b5638a4c0""
                        }";
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.PostAsync("https://api.packet.net/projects/ca73364c-6023-4935-9137-2132e73c20b4/devices", content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            object responseBodyObject = JsonConvert.DeserializeObject(responseBody);
            Console.WriteLine(responseBodyObject);
            return responseBodyObject;
        }
        
        public static object DeserializeFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize(jsonTextReader);
            }
        }
    }
}
