using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace SampleIdentityServer
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "jane",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "john",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("testapi", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            // Service account client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client.deligate.jane",
                    AllowedGrantTypes = new List<string>() { "serviceaccount" } ,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "testapi" },
                    Properties = new Dictionary<string, string>()
                    {
                        {"subject","1"}  // the user we are going to allow to be deligated.
                    }
                },
                new Client
                {
                    ClientId = "client.deligate.john",
                    AllowedGrantTypes = new List<string>() { "serviceaccount" } ,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "testapi" },
                    Properties = new Dictionary<string, string>()
                    {
                        {"subject","2"}  // the user we are going to allow to be deligated.
                    }
                }
            };
        }
    }
}