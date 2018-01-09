namespace Georadix.WebApi.Testing
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class FakeResponseHandlerFixture : IDisposable
    {
        private const string EmptyRequestContent = null;
        private const string NonEmptyRequestContent = "{ 'property': 'Not Empty' }";
        private const string ResponseContent = "Success";

        private static readonly Uri GetUri = new Uri("http://example.com?a=1&b=2");
        private static readonly Uri PostUri = new Uri("http://example.com");

        private FakeResponseHandler handler;

        public FakeResponseHandlerFixture()
        {
            this.handler = new FakeResponseHandler();
        }

        [Fact]
        public async Task AddFakeGetResponseOverridesGetResponse()
        {
            this.handler.AddFakeGetResponse(
                GetUri,
                () => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(ResponseContent, Encoding.Unicode, "application/json")
                });

            using (var client = this.CreateHttpClient())
            {
                var response = await client.GetStringAsync(GetUri);

                Assert.Equal(ResponseContent, response);
            }
        }

        [Fact]
        public async Task AddFakePostResponseOverridesEmptyPostResponse()
        {
            this.handler.AddFakePostResponse(
                PostUri,
                (requestContent) =>
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(ResponseContent, Encoding.Unicode, "application/json")
                    };
                });

            using (var client = this.CreateHttpClient())
            {
                var message = await client.PostAsJsonAsync(PostUri, EmptyRequestContent);

                var response = await message.Content.ReadAsStringAsync();

                Assert.Equal(ResponseContent, response);
            }
        }

        [Fact]
        public async Task AddFakePostResponseOverridesNonEmptyPostResponse()
        {
            this.handler.AddFakePostResponse(
                PostUri,
                (requestContent) =>
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(ResponseContent, Encoding.Unicode, "application/json")
                    };
                });

            using (var client = this.CreateHttpClient())
            {
                var message = await client.PostAsJsonAsync(PostUri, NonEmptyRequestContent);

                var response = await message.Content.ReadAsStringAsync();

                Assert.Equal(ResponseContent, response);
            }
        }

        public void Dispose()
        {
            if (this.handler != null)
            {
                this.handler.Dispose();
            }
        }

        [Fact]
        public async Task UnmappedUriReturnsNotFound()
        {
            using (var client = this.CreateHttpClient())
            {
                var message = await client.GetAsync(new Uri("http://unmapped.com"));

                Assert.Equal(HttpStatusCode.NotFound, message.StatusCode);
            }
        }

        private HttpClient CreateHttpClient()
        {
            return HttpClientFactory.Create(this.handler);
        }
    }
}