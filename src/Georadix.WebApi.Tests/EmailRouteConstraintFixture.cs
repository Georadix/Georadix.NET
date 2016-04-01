namespace Georadix.WebApi.Routing
{
    using Moq;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http.Routing;
    using Xunit;

    public class EmailRouteConstraintFixture
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("invalidemail@", false)]
        [InlineData("user@domain.com", true)]
        [InlineData("first.last@domain.gc.ca", true)]
        public void MatchReturnsProperResponse(string email, bool isMatch)
        {
            var sut = new EmailRouteConstraint();

            var parameterName = "email";
            var values = new Dictionary<string, object>();

            values.Add(parameterName, email);

            Assert.Equal(
                isMatch,
                sut.Match(
                    new HttpRequestMessage(),
                    Mock.Of<IHttpRoute>(),
                    parameterName,
                    values,
                    HttpRouteDirection.UriGeneration));
        }

        [Fact]
        public void MatchWithNoEmailParamDoesNotMatch()
        {
            var sut = new EmailRouteConstraint();

            Assert.False(
                sut.Match(
                    new HttpRequestMessage(),
                    Mock.Of<IHttpRoute>(),
                    "email",
                    new Dictionary<string, object>(),
                    HttpRouteDirection.UriGeneration));
        }
    }
}