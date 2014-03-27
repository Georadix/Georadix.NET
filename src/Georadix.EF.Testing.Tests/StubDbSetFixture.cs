namespace Georadix.EF.Testing.Tests
{
    using Georadix.Core.Domain;
    using System;
    using System.Collections;
    using Xunit;

    public class StubDbSetFixture
    {
        [Fact]
        public void AddItemAddsItToLocalCollection()
        {
            var sut = new StubDbSet<TestEntity>();
            var e = new TestEntity(Guid.NewGuid(), "E1");

            sut.Add(e);

            Assert.Same(e, sut.Local[0]);
        }

        [Fact]
        public void AttachItemAddsItToLocalCollection()
        {
            var sut = new StubDbSet<TestEntity>();
            var e = new TestEntity(Guid.NewGuid(), "E1");

            sut.Attach(e);

            Assert.Same(e, sut.Local[0]);
        }

        [Fact]
        public void CreateDerivedEntityReturnsDerivedEntityInstance()
        {
            var sut = new StubDbSet<TestEntity>();

            var e = sut.Create<DerivedTestEntity>();

            Assert.NotNull(e);
        }

        [Fact]
        public void CreateIsInitializedProperly()
        {
            var sut = new StubDbSet<TestEntity>();

            Assert.NotNull(sut.ElementType);
            Assert.Equal(typeof(TestEntity), sut.ElementType);
            Assert.NotNull(sut.Expression);
            Assert.NotNull(sut.GetEnumerator());
            Assert.NotNull(((IEnumerable)sut).GetEnumerator());
            Assert.NotNull(sut.Provider);
        }

        [Fact]
        public void CreateReturnsEntityInstance()
        {
            var sut = new StubDbSet<TestEntity>();

            var e = sut.Create();

            Assert.NotNull(e);
        }

        [Fact]
        public void DetachRemoveItemsFromLocalCollection()
        {
            var sut = new StubDbSet<TestEntity>();

            var e = new TestEntity();

            sut.Add(e);

            Assert.Same(e, sut.Local[0]);

            sut.Detach(e);

            Assert.Empty(sut.Local);
        }

        [Fact]
        public void FindWithKnowIdReturnsEntity()
        {
            var sut = new StubDbSet<TestEntity>();

            var e = new TestEntity(Guid.NewGuid(), "Name");

            sut.Add(e);

            var r = sut.Find(e.Id);

            Assert.Same(e, r);
        }

        [Fact]
        public void FindWithUnknownIdReturnsNull()
        {
            var sut = new StubDbSet<TestEntity>();

            var e = new TestEntity(Guid.NewGuid(), "Name");

            sut.Add(e);

            var r = sut.Find(Guid.Empty);

            Assert.Null(r);
        }

        [Fact]
        public void RemoveItemsFromLocalCollection()
        {
            var sut = new StubDbSet<TestEntity>();

            var e = new TestEntity();

            sut.Add(e);

            Assert.Same(e, sut.Local[0]);

            sut.Remove(e);

            Assert.Empty(sut.Local);
        }

        private class DerivedTestEntity : TestEntity
        {
            public DerivedTestEntity()
            {
            }
        }

        private class TestEntity : IEntity
        {
            public TestEntity(Guid id, string name)
            {
                this.Id = id;
                this.Name = name;
            }

            public TestEntity()
            {
            }

            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}