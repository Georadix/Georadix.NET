namespace Georadix.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class LinqExtensionsFixture
    {
        [Fact]
        public void AddRangeAddsSpecifiedItems()
        {
            ICollection<int> sut = new List<int>();
            var items = new int[] { 0, 1, 2, 3 };

            sut.AddRange(items);

            Assert.Equal(items, sut.ToArray());
        }

        [Fact]
        public void AddRangeOnNullCollectionThrowsArgumentNullException()
        {
            ICollection<int> sut = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.AddRange(null));

            Assert.Equal("collection", ex.ParamName);
        }

        [Fact]
        public void AddRangeWithNullItemsDoesNothing()
        {
            ICollection<int> sut = new List<int>();

            sut.AddRange(null);
        }

        [Fact]
        public void ForEachOnNullEnumerableThrowsArgumentNullException()
        {
            IEnumerable<int> sut = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.ForEach(i => i++));

            Assert.Equal("collection", ex.ParamName);
        }

        [Fact]
        public void ForEachPerformsAction()
        {
            IEnumerable<int> sut = new int[] { 0, 1, 2, 3 };
            var items = new List<int>();

            sut.ForEach(i => items.Add(i));

            Assert.Equal(sut, items.ToArray());
        }

        [Fact]
        public void ForEachWithNullActionDoesNothing()
        {
            IEnumerable<int> sut = new List<int>();

            sut.ForEach(null);
        }

        [Fact]
        public void MaxByOnEmptyEnumerableThrowsInvalidOperation()
        {
            var sut = new List<int>();

            var ex = Assert.Throws<InvalidOperationException>(() => sut.MaxBy(i => i));

            Assert.NotNull(ex.Message);

            var ex2 = Assert.Throws<InvalidOperationException>(() => sut.MaxBy(i => i, Comparer<int>.Default));

            Assert.Equal(ex.Message, ex2.Message);
        }

        [Fact]
        public void MaxByOnNullEnumerableThrowsArgumentNullException()
        {
            IEnumerable<int> sut = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.MaxBy(i => i));

            Assert.Equal("source", ex.ParamName);

            var ex2 = Assert.Throws<ArgumentNullException>(() => sut.MaxBy(i => i, Comparer<int>.Default));

            Assert.Equal(ex.ParamName, ex2.ParamName);
        }

        [Fact]
        public void MaxByWithCustomComparerReturnsExpectedResult()
        {
            var sut = new List<string>();
            sut.Add("zero");
            sut.Add("one");
            sut.Add("two");

            Assert.Equal(sut[1], sut.MaxBy(t => t, new ReverseComparer()));
        }

        [Fact]
        public void MaxByWithDefaultComparerReturnsExpectedResult()
        {
            var sut = new List<Tuple<int, string>>();
            sut.Add(new Tuple<int, string>(0, "zero"));
            sut.Add(new Tuple<int, string>(1, "one"));
            sut.Add(new Tuple<int, string>(2, "two"));

            var expectedByItem1 = sut.Last();
            var expectedByItem2 = sut.First();

            Assert.Equal(expectedByItem1, sut.MaxBy(t => t.Item1));
            Assert.Equal(expectedByItem2, sut.MaxBy(t => t.Item2));

            Assert.Equal(expectedByItem1, sut.MaxBy(t => t.Item1, Comparer<int>.Default));
            Assert.Equal(expectedByItem2, sut.MaxBy(t => t.Item2, Comparer<string>.Default));
        }

        [Fact]
        public void MaxByWithNullComparerThrowsArgumentNullException()
        {
            var sut = new List<int>();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.MaxBy(i => i, null));

            Assert.Equal("comparer", ex.ParamName);
        }

        [Fact]
        public void MaxByWithNullSelectorThrowsArgumentNullException()
        {
            var sut = new List<int>();
            Func<int, int> selector = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.MaxBy(selector));

            Assert.Equal("selector", ex.ParamName);

            var ex2 = Assert.Throws<ArgumentNullException>(() => sut.MaxBy(selector, Comparer<int>.Default));

            Assert.Equal(ex.ParamName, ex2.ParamName);
        }

        [Fact]
        public void MinByOnEmptyEnumerableThrowsInvalidOperation()
        {
            var sut = new List<int>();

            var ex = Assert.Throws<InvalidOperationException>(() => sut.MinBy(i => i));

            Assert.NotNull(ex.Message);

            var ex2 = Assert.Throws<InvalidOperationException>(() => sut.MinBy(i => i, Comparer<int>.Default));

            Assert.Equal(ex.Message, ex2.Message);
        }

        [Fact]
        public void MinByOnNullEnumerableThrowsArgumentNullException()
        {
            IEnumerable<int> sut = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.MinBy(i => i));

            Assert.Equal("source", ex.ParamName);

            var ex2 = Assert.Throws<ArgumentNullException>(() => sut.MinBy(i => i, Comparer<int>.Default));

            Assert.Equal(ex.ParamName, ex2.ParamName);
        }

        [Fact]
        public void MinByWithCustomComparerReturnsExpectedResult()
        {
            var sut = new List<string>();
            sut.Add("zero");
            sut.Add("one");
            sut.Add("two");

            Assert.Equal(sut.First(), sut.MinBy(t => t, new ReverseComparer()));
        }

        [Fact]
        public void MinByWithDefaultComparerReturnsExpectedResult()
        {
            var sut = new List<Tuple<int, string>>();
            sut.Add(new Tuple<int, string>(0, "zero"));
            sut.Add(new Tuple<int, string>(1, "one"));
            sut.Add(new Tuple<int, string>(2, "two"));

            var expectedByItem1 = sut.First();
            var expectedByItem2 = sut[1];

            Assert.Equal(expectedByItem1, sut.MinBy(t => t.Item1));
            Assert.Equal(expectedByItem2, sut.MinBy(t => t.Item2));

            Assert.Equal(expectedByItem1, sut.MinBy(t => t.Item1, Comparer<int>.Default));
            Assert.Equal(expectedByItem2, sut.MinBy(t => t.Item2, Comparer<string>.Default));
        }

        [Fact]
        public void MinByWithNullComparerThrowsArgumentNullException()
        {
            var sut = new List<int>();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.MinBy(i => i, null));

            Assert.Equal("comparer", ex.ParamName);
        }

        [Fact]
        public void MinByWithNullSelectorThrowsArgumentNullException()
        {
            var sut = new List<int>();
            Func<int, int> selector = null;

            var ex = Assert.Throws<ArgumentNullException>(() => sut.MinBy(selector));

            Assert.Equal("selector", ex.ParamName);

            var ex2 = Assert.Throws<ArgumentNullException>(() => sut.MinBy(selector, Comparer<int>.Default));

            Assert.Equal(ex.ParamName, ex2.ParamName);
        }

        private class ReverseComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return y.CompareTo(x);
            }
        }
    }
}