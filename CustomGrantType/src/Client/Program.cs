using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public static class ServiceAccountRequiredProperty
    {
        public const string Subject = "subject";
    }

    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();
        
        private static async Task MainAsync()
        {
            // Grab endpoints from the Identity server.
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // Create custom paramters for service account
            var user = new Dictionary<string, string>
            {
                { ServiceAccountRequiredProperty.Subject, "1"}
            };

            // Init the sercie account client.
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client.deligate.jane", "secret");  
            // Request a token for this user.
            var tokenResponse = await tokenClient.RequestCustomGrantAsync("serviceaccount", "testapi", user);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/user");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            Console.ReadLine();
        }
    }
}