namespace Georadix.WebApi.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A handler that returns predefined fake response messages to HTTP requests.
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, Func<HttpResponseMessage>> fakeGetResponses
            = new Dictionary<Uri, Func<HttpResponseMessage>>();

        private readonly Dictionary<Uri, Func<string, HttpResponseMessage>> fakePostResponses
            = new Dictionary<Uri, Func<string, HttpResponseMessage>>();

        /// <summary>
        /// Adds a fake response message to a GET at a specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="predicate">The delegate that returns the response message.</param>
        public void AddFakeGetResponse(Uri uri, Func<HttpResponseMessage> predicate)
        {
            this.fakeGetResponses.Add(uri, predicate);
        }

        /// <summary>
        /// Adds a fake response message to a POST at a specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="predicate">The delegate that returns the response message.</param>
        public void AddFakePostResponse(Uri uri, Func<string, HttpResponseMessage> predicate)
        {
            this.fakePostResponses.Add(uri, predicate);
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="Task{T}" />. The task object representing the asynchronous operation.
        /// </returns>
        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var decodedRequestUri = new Uri(WebUtility.UrlDecode(request.RequestUri.ToString()));

            if ((request.Method == HttpMethod.Get) && this.fakeGetResponses.ContainsKey(decodedRequestUri))
            {
                return await Task.FromResult(this.fakeGetResponses[decodedRequestUri]());
            }
            else if ((request.Method == HttpMethod.Post) && this.fakePostResponses.ContainsKey(decodedRequestUri))
            {
                var content = (request.Content == null) ? null : await request.Content.ReadAsStringAsync();
                return await Task.FromResult(this.fakePostResponses[decodedRequestUri](content));
            }
            else
            {
                return await Task.FromResult(
                    new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
        }
    }
}