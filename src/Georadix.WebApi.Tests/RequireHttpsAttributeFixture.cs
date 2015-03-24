namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;
    using Xunit.Extensions;

    public class RequireHttpsAttributeFixture
    {
        public const string DeleteResource = BaseUrl + "delete-resource";
        public const string GetResource = BaseUrl + "get-resource";
        public const string HeadResource = BaseUrl + "head-resource";
        public const string PostResource = BaseUrl + "post-resource";
        public const string PutResource = BaseUrl + "put-resource";
        private const string BaseUrl = "RequireHttpsAttributeFixtureController/";

        public static IEnumerable<object[]> Scenarios
        {
            get
            {
                return new object[][]
                {
                    new object[] { HttpMethod.Delete, true, HttpStatusCode.NoContent },
                    new object[] { HttpMethod.Get, true, HttpStatusCode.NoContent },
                    new object[] { HttpMethod.Head, true, HttpStatusCode.NoContent },
                    new object[] { HttpMethod.Post, true, HttpStatusCode.NoContent },
                    new object[] { HttpMethod.Put, true, HttpStatusCode.NoContent },
                    new object[] { HttpMethod.Delete, false, HttpStatusCode.NotFound },
                    new object[] { HttpMethod.Get, false, HttpStatusCode.Redirect },
                    new object[] { HttpMethod.Head, false, HttpStatusCode.Redirect },
                    new object[] { HttpMethod.Post, false, HttpStatusCode.NotFound },
                    new object[] { HttpMethod.Put, false, HttpStatusCode.NotFound }
                };
            }
        }

        [Fact]
        public void ConstructorWithHttpsPortLessThan0ThrowsArgumentOutOfRangeException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new RequireHttpsAttribute(-1));

            Assert.Equal("httpsPort", ex.ParamName);
        }

        [Theory]
        [PropertyData("Scenarios")]
        public async Task ExecuteActionReturnsExpectedResult(
            HttpMethod httpMethod, bool useHttps, HttpStatusCode expectedStatusCode)
        {
            using (var server = this.CreateServer(useHttps))
            {
                HttpResponseMessage response = null;

                if (httpMethod == HttpMethod.Delete)
                {
                    response = await server.Client.DeleteAsync(DeleteResource);
                }
                else if (httpMethod == HttpMethod.Get)
                {
                    response = await server.Client.GetAsync(GetResource);
                }
                else if (httpMethod == HttpMethod.Head)
                {
                    response = await server.Client.SendAsync(new HttpRequestMessage(HttpMethod.Head, HeadResource));
                }
                else if (httpMethod == HttpMethod.Post)
                {
                    response = await server.Client.PostAsJsonAsync(PostResource, string.Empty);
                }
                else if (httpMethod == HttpMethod.Put)
                {
                    response = await server.Client.PutAsJsonAsync(PutResource, string.Empty);
                }

                Assert.Equal(expectedStatusCode, response.StatusCode);

                if (!useHttps)
                {
                    var location = response.RequestMessage.RequestUri.ToString().Replace("http://", "https://");

                    if (httpMethod != HttpMethod.Head)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        Assert.Contains("The resource can be found at", responseContent);
                        Assert.Contains(location, responseContent);
                    }

                    if ((httpMethod == HttpMethod.Get) || (httpMethod == HttpMethod.Head))
                    {
                        Assert.Equal(location, response.Headers.Location.AbsoluteUri);
                    }
                }
            }
        }

        private InMemoryServer CreateServer(bool useHttps)
        {
            var server = new InMemoryServer(useHttps);

            server.Configuration.Filters.Add(new RequireHttpsAttribute());
            server.Configuration.MapHttpAttributeRoutes();

            return server;
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    public class RequireHttpsAttributeFixtureController : ApiController
    {
        [Route(RequireHttpsAttributeFixture.DeleteResource)]
        public void Delete()
        {
        }

        [Route(RequireHttpsAttributeFixture.GetResource)]
        public void Get()
        {
        }

        [Route(RequireHttpsAttributeFixture.HeadResource)]
        public void Head()
        {
        }

        [Route(RequireHttpsAttributeFixture.PostResource)]
        public void Post()
        {
        }

        [Route(RequireHttpsAttributeFixture.PutResource)]
        public void Put()
        {
        }
    }
}