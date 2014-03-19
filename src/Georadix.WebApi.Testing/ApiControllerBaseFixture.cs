namespace Georadix.WebApi.Testing
{
    using Georadix.Services;
    using Moq;
    using SimpleInjector;
    using System;
    using System.Web.Http;

    /// <summary>
    /// A base fixture to facilitate unit testing <see cref="ApiController"/> within an <see cref="InMemoryServer"/>.
    /// </summary>
    /// <typeparam name="TController">The type of controller.</typeparam>
    public abstract class ApiControllerBaseFixture<TController> : IDisposable where TController : ApiController
    {
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBaseFixture{TController}" /> class.
        /// </summary>
        /// <remarks>
        /// Use the server configuration callback in order to perform configuration usually done
        /// on application start in a standard Web API project.
        /// </remarks>
        /// <param name="serverConfigurationCallback">The server configuration callback.</param>
        public ApiControllerBaseFixture(Action<InMemoryServer> serverConfigurationCallback)
        {
            this.Server = new InMemoryServer(new Container(), serverConfigurationCallback);

            this.CommandServiceMock = new Mock<ICommandService>(MockBehavior.Strict);
            this.QueryServiceMock = new Mock<IQueryService>(MockBehavior.Strict);

            this.Server.Container.RegisterSingle<ICommandService>(this.CommandServiceMock.Object);
            this.Server.Container.RegisterSingle<IQueryService>(this.QueryServiceMock.Object);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ApiControllerBaseFixture{TController}"/> class.
        /// </summary>
        ~ApiControllerBaseFixture()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the command service mock.
        /// </summary>
        /// <value>
        /// The command service mock.
        /// </value>
        protected Mock<ICommandService> CommandServiceMock { get; private set; }

        /// <summary>
        /// Gets the query service mock.
        /// </summary>
        /// <value>
        /// The query service mock.
        /// </value>
        protected Mock<IQueryService> QueryServiceMock { get; private set; }

        /// <summary>
        /// Gets the server hosting the controller in memory.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        protected InMemoryServer Server { get; private set; }

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
                    this.Server.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}