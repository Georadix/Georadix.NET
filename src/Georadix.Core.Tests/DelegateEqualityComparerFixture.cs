namespace Georadix.Core
{
    using System;
    using Xunit;

    public class DelegateEqualityComparerFixture
    {
        private readonly Func<string, string, bool> defaultComparer = (string x, string y) => { return true; };
        private readonly Func<string, int> defaultHash = (string obj) => { return 0; };

        [Fact]
        public void CreateWithNullComparerThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new DelegateEqualityComparer<string>(null, this.defaultHash);
            });

            Assert.Equal("comparer", ex.ParamName);
        }

        [Fact]
        public void CreateWithNullHashThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                new DelegateEqualityComparer<string>(this.defaultComparer, null);
            });

            Assert.Equal("hash", ex.ParamName);
        }

        [Fact]
        public void EqualsCallsTheProvidedDelegate()
        {
            var comparerWasCalled = false;

            Func<string, string, bool> comparer = (string x, string y) =>
            {
                comparerWasCalled = true;
                return true;
            };

            var sut = new DelegateEqualityComparer<string>(comparer, this.defaultHash);

            sut.Equals("test", "test");

            Assert.True(comparerWasCalled);
        }

        [Fact]
        public void GetHashCodeCallsTheProvidedDelegate()
        {
            var hashWasCalled = false;

            Func<string, int> hash = (string obj) =>
            {
                hashWasCalled = true;
                return 1;
            };

            var sut = new DelegateEqualityComparer<string>(this.defaultComparer, hash);

            sut.GetHashCode("test");

            Assert.True(hashWasCalled);
        }
    }
}