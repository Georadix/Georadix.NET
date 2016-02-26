namespace Georadix.WebApi
{
    using Georadix.WebApi.Filters;
    using Georadix.WebApi.Testing;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Xunit;
    using Xunit.Extensions;

    public class CustomAuthorizationAttributeFixture
    {
        public const string AdminResource = BaseUrl + "admin-resource";
        public const string ProtectedResource = BaseUrl + "protected-resource";
        public const string PublicResource = BaseUrl + "public-resource";
        private const string BaseUrl = "customAuthorizationAttributeFixtureController/";

        private static IPrincipal adminPrincipal = new GenericPrincipal(
            new GenericIdentity("Admin1", "Custom"), new string[] { "Admin" });

        private static IPrincipal anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

        private static IPrincipal userPrincipal = new GenericPrincipal(
            new GenericIdentity("User1", "Custom"), new string[] { "User" });

        public static IEnumerable<object[]> AuthenticatedPrincipals
        {
            get
            {
                return new object[][]
                {
                    new object[] { adminPrincipal },
                    new object[] { userPrincipal }
                };
            }
        }

        public static IEnumerable<object[]> Principals
        {
            get
            {
                return new object[][]
                {
                    new object[] { adminPrincipal },
                    new object[] { anonymousPrincipal },
                    new object[] { userPrincipal }
                };
            }
        }

        [Fact]
        public async Task GetAdminResourceWithAdminPrincipalReturnsIt()
        {
            using (var server = this.CreateServer(() => adminPrincipal))
            {
                var response = await server.Client.GetAsync(AdminResource);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var content = await response.Content.ReadAsAsync<string>();

                Assert.Equal(AdminResource, content);
            }
        }

        [Fact]
        public async Task GetAdminResourceWithUserPrincipalReturnsForbidden()
        {
            using (var server = this.CreateServer(() => userPrincipal))
            {
                var response = await server.Client.GetAsync(AdminResource);

                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetProtectedResourceWithAnonymousPrincipalReturnsUnauthorized()
        {
            using (var server = this.CreateServer(() => anonymousPrincipal))
            {
                var response = await server.Client.GetAsync(ProtectedResource);

                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Theory]
        [MemberData("AuthenticatedPrincipals")]
        public async Task GetProtectedResourceWithAuthenticatedPrincipalReturnsIt(IPrincipal authenticatedPrincipal)
        {
            using (var server = this.CreateServer(() => authenticatedPrincipal))
            {
                var response = await server.Client.GetAsync(ProtectedResource);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var content = await response.Content.ReadAsAsync<string>();

                Assert.Equal(ProtectedResource, content);
            }
        }

        [Fact]
        public async Task GetProtectedResourceWithNullPrincipalReturnsUnauthorized()
        {
            using (var server = this.CreateServer(() => null))
            {
                var response = await server.Client.GetAsync(ProtectedResource);

                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Theory]
        [MemberData("Principals")]
        public async Task GetPublicResourceWithAnyPrincipalReturnsIt(IPrincipal principal)
        {
            using (var server = this.CreateServer(() => principal))
            {
                var response = await server.Client.GetAsync(PublicResource);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var content = await response.Content.ReadAsAsync<string>();

                Assert.Equal(PublicResource, content);
            }
        }

        private InMemoryServer CreateServer(Func<IPrincipal> getPrincipal)
        {
            var server = new InMemoryServer();

            var handler = new AuthorizationTestMessageHandler();
            handler.GetPrincipal = getPrincipal;

            server.Configuration.MessageHandlers.Add(handler);
            server.Configuration.MapHttpAttributeRoutes();

            return server;
        }

        private class AuthorizationTestMessageHandler : DelegatingHandler
        {
            public Func<IPrincipal> GetPrincipal { get; set; }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.GetRequestContext().Principal = this.GetPrincipal();

                return base.SendAsync(request, cancellationToken);
            }
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Embedding this controller class inside the fixture causes routing to its methods to fail.")]
    public class CustomAuthorizationAttributeFixtureController : ApiController
    {
        [Route(CustomAuthorizationAttributeFixture.AdminResource)]
        [CustomAuthorize(Roles = "Admin")]
        public string GetAdminResource()
        {
            return CustomAuthorizationAttributeFixture.AdminResource;
        }

        [Route(CustomAuthorizationAttributeFixture.ProtectedResource)]
        [CustomAuthorize]
        public string GetProtectedResource()
        {
            return CustomAuthorizationAttributeFixture.ProtectedResource;
        }

        [Route(CustomAuthorizationAttributeFixture.PublicResource)]
        [AllowAnonymous]
        [CustomAuthorize]
        public string GetPublicResource()
        {
            return CustomAuthorizationAttributeFixture.PublicResource;
        }
    }
}