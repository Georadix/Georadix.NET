namespace Georadix.Core.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Xunit;
    using Xunit.Extensions;

    public class ClaimsExtensionsFixture
    {
        public static IEnumerable<object[]> HasClaimsScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new Claim[]
                        {
                            new Claim("one", "one")
                        },
                        true
                    },
                    new object[]
                    {
                        new Claim[]
                        {
                            new Claim("one", "one"),
                            new Claim("two", "two")
                        },
                        true
                    },
                    new object[]
                    {
                        new Claim[]
                        {
                            new Claim("one", "one"),
                            new Claim("two", "two"),
                            new Claim("three", "three")
                        },
                        false
                    },
                    new object[]
                    {
                        new Claim[]
                        {
                            new Claim("four", "four")
                        },
                        false
                    }
                };
            }
        }

        public static IEnumerable<object[]> HasClaimTypesScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new string[] { "one" },
                        true
                    },
                    new object[]
                    {
                        new string[] { "one", "two" },
                        true
                    },
                    new object[]
                    {
                        new string[] { "one", "two", "three" },
                        false
                    },
                    new object[]
                    {
                        new string[] { "four" },
                        false
                    }
                };
            }
        }

        [Theory]
        [MemberData("HasClaimsScenarios")]
        public void HasClaimsReturnsExpectedResult(IEnumerable<Claim> claims, bool expected)
        {
            var principalClaims = new List<Claim>
            {
                new Claim("one", "one"),
                new Claim("two", "two")
            };

            var sut = new ClaimsPrincipal(new ClaimsIdentity(principalClaims));

            Assert.Equal(expected, sut.HasClaims(claims));
        }

        [Fact]
        public void HasClaimsWithNullClaimItemsThrowsArgumentException()
        {
            var sut = new ClaimsPrincipal();

            var ex = Assert.Throws<ArgumentException>(() => sut.HasClaims(new Claim[] { null }));
            Assert.Equal("claims", ex.ParamName);
        }

        [Fact]
        public void HasClaimsWithNullClaimsThrowsArgumentNullException()
        {
            var sut = new ClaimsPrincipal();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.HasClaims(null));
            Assert.Equal("claims", ex.ParamName);
        }

        [Fact]
        public void HasClaimsWithNullClaimTypeItemsThrowsArgumentException()
        {
            var sut = new ClaimsPrincipal();

            var ex = Assert.Throws<ArgumentException>(() => sut.HasClaimTypes(new string[] { null }));
            Assert.Equal("claimTypes", ex.ParamName);
        }

        [Theory]
        [MemberData("HasClaimTypesScenarios")]
        public void HasClaimTypesReturnsExpectedResult(IEnumerable<string> claimTypes, bool expected)
        {
            var principalClaims = new List<Claim>
            {
                new Claim("one", "one"),
                new Claim("two", "two")
            };

            var sut = new ClaimsPrincipal(new ClaimsIdentity(principalClaims));

            Assert.Equal(expected, sut.HasClaimTypes(claimTypes));
        }

        [Fact]
        public void HasClaimTypesWithNullClaimTypesThrowsArgumentNullException()
        {
            var sut = new ClaimsPrincipal();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.HasClaimTypes(null));
            Assert.Equal("claimTypes", ex.ParamName);
        }
    }
}