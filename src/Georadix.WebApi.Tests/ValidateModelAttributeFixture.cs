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
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;

    public class ValidateModelAttributeFixture
    {
        [Fact]
        public async Task ExecuteActionWithInvalidModelReturnsModelState()
        {
            using (var server = this.CreateServer())
            {
                var model = new ValidateModelAttributeFixtureModel() { Name = string.Empty };

                var response = await server.Client.PostAsJsonAsync("entities", model);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();

                Assert.Contains("The name cannot be empty", responseContent);
            }
        }

        [Fact]
        public async Task ExecuteActionWithValidModelReturnsContent()
        {
            using (var server = this.CreateServer())
            {
                var model = new ValidateModelAttributeFixtureModel() { Name = "test" };

                var response = await server.Client.PostAsJsonAsync("entities", model);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var responseContent = await response.Content.ReadAsStringAsync();

                Assert.Contains(model.Name, responseContent);
            }
        }

        private void ConfigureServer(InMemoryServer server)
        {
            server.Configuration.Filters.Add(new ValidateModelAttribute());

            server.Configuration.MapHttpAttributeRoutes();
        }

        private InMemoryServer CreateServer()
        {
            var container = new Container();

            container.Register<ValidateModelAttributeFixtureController>();

            var server = new InMemoryServer(container, this.ConfigureServer);

            return server;
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    [RoutePrefix("entities")]
    public class ValidateModelAttributeFixtureController : ApiController
    {
        [Route("")]
        public string Post(ValidateModelAttributeFixtureModel model)
        {
            return model.Name;
        }
    }

    public class ValidateModelAttributeFixtureModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The name cannot be empty.")]
        public string Name { get; set; }
    }
}