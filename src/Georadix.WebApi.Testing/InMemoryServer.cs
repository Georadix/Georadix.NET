namespace Georadix.WebApi.Testing
{
    using log4net;
    using Moq;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Host Web API endpoints in memory to facilitate testing <see cref="ApiController"/>.
    /// </summary>
    public class InMemoryServer : IDisposable
    {
        private readonly Dictionary<Type, Mock<ILog>> mockLoggers = new Dictionary<Type, Mock<ILog>>();
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryServer" /> class with a specified container.
        /// </summary>
        /// <remarks>
        /// Use the server configuration callback in order to perform configuration usually done
        /// on application start in a standard Web API project.
        /// </remarks>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" /></exception>
        public InMemoryServer(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Configuration = new HttpConfiguration()
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
            };

            var server = new HttpServer(this.Configuration);

            this.Client = new HttpClient(server, true)
            {
                BaseAddress = new Uri("http://www.test.com/")
            };

            this.Container = container;

            this.LoggerFactory = new Func<Type, ILog>(type =>
            {
                if (!this.MockLoggers.ContainsKey(type))
                {
                    this.MockLoggers.Add(type, new Mock<ILog>());
                }

                return this.MockLoggers[type].Object;
            });

            this.Container.RegisterSingle<Func<Type, ILog>>(this.LoggerFactory);

            this.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this.Container);
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
        /// Gets the container.
        /// </summary>
        public Container Container { get; private set; }

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        public Func<Type, ILog> LoggerFactory { get; private set; }

        /// <summary>
        /// Gets the mocked loggers.
        /// </summary>
        public Dictionary<Type, Mock<ILog>> MockLoggers
        {
            get { return this.mockLoggers; }
        }

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