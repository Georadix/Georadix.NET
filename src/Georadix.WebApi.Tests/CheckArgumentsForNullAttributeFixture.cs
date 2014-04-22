namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using Newtonsoft.Json.Linq;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;
    using Xunit.Extensions;

    public class CheckArgumentsForNullAttributeFixture
    {
        private readonly string rootRoute;

        public CheckArgumentsForNullAttributeFixture()
        {
            this.rootRoute = string.Format(
                "{0}", typeof(CheckArgumentsForNullAttributeFixtureController).Name.ToLowerInvariant());
        }

        [Fact]
        public async Task ExecuteActionWithNonNullArgumentsReturnsOK()
        {
            using (var server = this.CreateServer())
            {
                CheckArgumentsForNullAttributeFixtureModel model =
                    new CheckArgumentsForNullAttributeFixtureModel() { Name = "Body" };

                var response = await server.Client.PostAsJsonAsync(this.rootRoute + "/items2?Name=Uri", model);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var responseContent = await response.Content.ReadAsAsync<string>();

                Assert.Equal("BodyUri", responseContent);
            }
        }

        [Theory]
        [InlineData("/items1", new string[] { "body", "uri", "number" })]
        [InlineData("/items2", new string[] { "body", "uri" })]
        [InlineData("/items3", new string[] { "body" })]
        [InlineData("/items4", new string[] { "uri" })]
        public async Task ExecuteActionWithNullArgumentsReturnsBadRequest(string route, string[] args)
        {
            using (var server = this.CreateServer())
            {
                CheckArgumentsForNullAttributeFixtureModel model = null;

                var response = await server.Client.PostAsJsonAsync(this.rootRoute + route, model);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsAsync<HttpError>();
                var errors = responseContent["ModelState"] as JObject;

                Assert.Equal(args.Length, errors.Count);

                foreach (var arg in args)
                {
                    Assert.Equal(1, errors[arg].Count());
                    Assert.Equal("The parameter is required.", errors[arg].Values<string>().Single());
                }
            }
        }

        private InMemoryServer CreateServer()
        {
            var server = new InMemoryServer();

            server.Configuration.MapHttpAttributeRoutes();

            return server;
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    [RoutePrefix("CheckArgumentsForNullAttributeFixtureController")]
    public class CheckArgumentsForNullAttributeFixtureController : ApiController
    {
        [Route("items1")]
        [CheckArgumentsForNull]
        public string Post2(
            [FromBody]CheckArgumentsForNullAttributeFixtureModel body,
            [FromUri]CheckArgumentsForNullAttributeFixtureModel uri,
            int? number = null)
        {
            return string.Concat(body.Name, uri.Name);
        }

        [Route("items2")]
        [CheckArgumentsForNull("body", "uri")]
        public string Post3(
            [FromBody]CheckArgumentsForNullAttributeFixtureModel body,
            [FromUri]CheckArgumentsForNullAttributeFixtureModel uri,
            int? number = null)
        {
            return string.Concat(body.Name, uri.Name);
        }

        [Route("items3")]
        [CheckArgumentsForNull("body")]
        public string Post4(
            [FromBody]CheckArgumentsForNullAttributeFixtureModel body,
            [FromUri]CheckArgumentsForNullAttributeFixtureModel uri,
            int? number = null)
        {
            return string.Concat(body.Name, uri == null ? string.Empty : uri.Name);
        }

        [Route("items4")]
        [CheckArgumentsForNull("uri")]
        public string Post5(
            [FromBody]CheckArgumentsForNullAttributeFixtureModel body,
            [FromUri]CheckArgumentsForNullAttributeFixtureModel uri,
            int? number = null)
        {
            return string.Concat(body == null ? string.Empty : body.Name, uri.Name);
        }
    }

    public class CheckArgumentsForNullAttributeFixtureModel
    {
        public string Name { get; set; }
    }
}