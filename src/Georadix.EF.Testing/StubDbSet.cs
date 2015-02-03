namespace Georadix.EF.Testing
{
    using Georadix.Core.Domain;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A database set stub to facilitate unit testing with Entity Framework.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class StubDbSet<T> : IDbSet<T> where T : class, IEntity
    {
        private readonly ObservableCollection<T> data;
        private readonly IQueryable query;

        /// <summary>
        /// Initializes a new instance of the <see cref="StubDbSet{T}"/> class.
        /// </summary>
        public StubDbSet()
        {
            this.data = new ObservableCollection<T>();
            this.query = this.data.AsQueryable();
        }

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance
        /// of <see cref="IQueryable" /> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="Type" /> that represents the type of the element(s) that are returned when
        /// the expression tree associated with this object is executed.</returns>
        public Type ElementType
        {
            get { return this.query.ElementType; }
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="IQueryable" />.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression" /> that is associated with this instance of <see cref="IQueryable" />.
        /// </returns>
        public Expression Expression
        {
            get { return this.query.Expression; }
        }

        /// <summary>
        /// Gets the local collection of entities.
        /// </summary>
        /// <value>
        /// The local collection of entities.
        /// </value>
        public ObservableCollection<T> Local
        {
            get { return this.data; }
        }

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>The <see cref="IQueryProvider" /> that is associated with this data source.</returns>
        public IQueryProvider Provider
        {
            get { return this.query.Provider; }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item that was added.</returns>
        public T Add(T item)
        {
            this.data.Add(item);
            return item;
        }

        /// <summary>
        /// Attaches the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item that was attached.</returns>
        public T Attach(T item)
        {
            this.data.Add(item);
            return item;
        }

        /// <summary>
        /// Creates an entity instance.
        /// </summary>
        /// <returns>An entity instance.</returns>
        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Creates a derived entity instance.
        /// </summary>
        /// <typeparam name="TDerivedEntity">The type of the derived entity.</typeparam>
        /// <returns>A derived entity instance.</returns>
        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        /// <summary>
        /// Detaches the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item that was detached.</returns>
        public T Detach(T item)
        {
            this.data.Remove(item);
            return item;
        }

        /// <summary>
        /// Finds the specified key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>An entity matching the key values if one is found; otherwise, <see langword="null"/>.</returns>
        public T Find(params object[] keyValues)
        {
            var ids = keyValues.Cast<Guid>().ToList();

            return this.data.FirstOrDefault((e) => { return ids.Contains(e.Id); });
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
            Justification = "The generic version of this method can be used by the derived classes.")]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The item that was removed.</returns>
        public T Remove(T item)
        {
            this.data.Remove(item);
            return item;
        }
    }
}