namespace SimpleInjector
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides extension methods that relies on a <see cref="DependencyContext"/> to register services.
    /// </summary>
    public static class ContextDependentExtensions
    {
        /// <summary>
        /// Registers with a context.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="contextBasedFactory">The context based factory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextBasedFactory"/> is <see langword="null"/>.
        /// </exception>
        public static void RegisterWithContext<TService>(
            this Container container, Func<DependencyContext, TService> contextBasedFactory)
            where TService : class
        {
            contextBasedFactory.AssertNotNull("contextBasedFactory");

            Func<TService> rootFactory = () =>
            {
                return contextBasedFactory(DependencyContext.Root);
            };

            container.Register<TService>(rootFactory, Lifestyle.Transient);

            // Allow the Func<DependencyContext, TService> to be
            // injected into parent types.
            container.ExpressionBuilding += (sender, e) =>
            {
                if (e.RegisteredServiceType != typeof(TService))
                {
                    var rewriter = new DependencyContextRewriter
                    {
                        ServiceType = e.RegisteredServiceType,
                        ContextBasedFactory = contextBasedFactory,
                        RootFactory = rootFactory,
                        Expression = e.Expression
                    };

                    e.Expression = rewriter.Visit(e.Expression);
                }
            };
        }

        [ExcludeFromCodeCoverage]
        private sealed class DependencyContextRewriter : ExpressionVisitor
        {
            internal object ContextBasedFactory { get; set; }

            internal Expression Expression { get; set; }

            internal Type ImplementationType
            {
                get
                {
                    var expression = this.Expression as NewExpression;

                    if (expression != null)
                    {
                        return expression.Constructor.DeclaringType;
                    }

                    return this.ServiceType;
                }
            }

            internal object RootFactory { get; set; }

            internal Type ServiceType { get; set; }

            protected override Expression VisitInvocation(
                InvocationExpression node)
            {
                if (!this.IsRootedContextBasedFactory(node))
                {
                    return base.VisitInvocation(node);
                }

                return Expression.Invoke(
                    Expression.Constant(this.ContextBasedFactory),
                    Expression.Constant(
                        new DependencyContext(
                            this.ServiceType,
                            this.ImplementationType)));
            }

            private bool IsRootedContextBasedFactory(
                InvocationExpression node)
            {
                var expression =
                    node.Expression as ConstantExpression;

                if (expression == null)
                {
                    return false;
                }

                return object.ReferenceEquals(expression.Value, this.RootFactory);
            }
        }
    }

    /// <summary>
    /// Represents the context of a dependency when injected.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Both are extensions that should be embedded inside the SimpleInjector code itself.")]
    [DebuggerDisplay("DependencyContext (ServiceType: {ServiceType}, ImplementationType: {ImplementationType})")]
    public class DependencyContext
    {
        internal static readonly DependencyContext Root = new DependencyContext();

        internal DependencyContext(Type serviceType, Type implementationType)
        {
            this.ServiceType = serviceType;
            this.ImplementationType = implementationType;
        }

        private DependencyContext()
        {
        }

        /// <summary>
        /// Gets the type of the implementation.
        /// </summary>
        /// <value>
        /// The type of the implementation.
        /// </value>
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public Type ServiceType { get; private set; }
    }
}