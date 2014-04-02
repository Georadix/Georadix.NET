namespace Georadix.WebApi.Testing
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an in memory content serializer to force content formatters when unit testing API calls
    /// with an <see cref="InMemoryServer"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InMemoryHttpContentSerializationHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryHttpContentSerializationHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">
        /// The inner handler which is responsible for processing the HTTP response messages.
        /// </param>
        public InMemoryHttpContentSerializationHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous 
        /// operation.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Content = await this.ConvertToStreamContentAsync(request.Content);

            var response = await base.SendAsync(request, cancellationToken);

            response.Content = await this.ConvertToStreamContentAsync(response.Content);

            return response;
        }

        private async Task<StreamContent> ConvertToStreamContentAsync(HttpContent originalContent)
        {
            if (originalContent == null)
            {
                return null;
            }

            var streamContent = originalContent as StreamContent;

            if (streamContent != null)
            {
                return streamContent;
            }

            var ms = new MemoryStream();

            await originalContent.CopyToAsync(ms);

            // Reset the stream position back to 0 as in the previous CopyToAsync() call,
            // a formatter for example, could have made the position to be at the end
            ms.Position = 0;

            streamContent = new StreamContent(ms);

            // copy headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in originalContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }
    }
}
