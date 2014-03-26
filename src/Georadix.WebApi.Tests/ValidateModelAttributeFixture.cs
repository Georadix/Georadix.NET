namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using Newtonsoft.Json;
    using SimpleInjector;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;

    public class ValidateModelAttributeFixture
    {
        [Fact]
        public async Task ExecuteActionWithValidModelReturnsContent()
        {
            using (var server = new InMemoryServer(new Container(), this.ConfigureServer))
            {
                server.Container.Register<ValidateModelAttributeFixtureController>();

                var model = new TestModel() { Name = "test" };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await server.Client.PostAsync("entities", content);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();

                Assert.Contains(model.Name, responseContent);
            }
        }

        [Fact]
        public async Task ExecuteActionWithInvalidModelReturnsModelState()
        {
            using (var server = new InMemoryServer(new Container(), this.ConfigureServer))
            {
                server.Container.Register<ValidateModelAttributeFixtureController>();

                var model = new TestModel() { Name = string.Empty };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var response = await server.Client.PostAsync("entities", content);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();

                Assert.Contains("The name cannot be empty", responseContent);
            }
        }

        private void ConfigureServer(InMemoryServer server)
        {
            server.Configuration.Filters.Add(new ValidateModelAttribute());

            server.Configuration.MapHttpAttributeRoutes();
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    [RoutePrefix("entities")]
    public class ValidateModelAttributeFixtureController : ApiController
    {
        [Route("")]
        public string Post(TestModel model)
        {
            return model.Name;
        }
    }

    public class TestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The name cannot be empty.")]
        public string Name { get; set; }
    }
}