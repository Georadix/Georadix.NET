namespace Georadix.WebApi.Testing
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Host Web API endpoints in memory to facilitate testing <see cref="ApiController"/>.
    /// </summary>
    public class InMemoryServer : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryServer"/> class.
        /// </summary>
        /// <param name="useHttps">A value indicating whether the client should use HTTPS.</param>
        public InMemoryServer(bool useHttps = false)
            : this(new Uri((useHttps ? Uri.UriSchemeHttps : Uri.UriSchemeHttp) + "://www.test.com/"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryServer"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseAddress"/> is <see langword="null"/>.
        /// </exception>
        public InMemoryServer(Uri baseAddress)
        {
            baseAddress.AssertNotNull("baseAddress");

            this.Configuration = new HttpConfiguration()
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
            };

            var server = new HttpServer(this.Configuration);

            this.Client = new HttpClient(new InMemoryHttpContentSerializationHandler(server), true)
            {
                BaseAddress = baseAddress
            };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="InMemoryServer"/> class.
        /// </summary>
        ~InMemoryServer()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public HttpConfiguration Configuration { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Client.Dispose();
                    this.Configuration.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}