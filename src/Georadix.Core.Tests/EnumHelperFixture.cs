namespace Georadix.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;

    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1602:EnumerationItemsMustBeDocumented",
        Justification = "Simple test enumeration does not require documentation.")]
    public class EnumHelperFixture
    {
        private enum TestEnum1
        {
            Value1 = 0,
            Value2,
            Value3
        }

        private enum TestEnum2
        {
            Value1 = 0,
            Value2,
            Value3
        }

        private enum TestEnum3
        {
            V1 = 0,
            V2,
            V3
        }

        [Fact]
        public void MapNullableReturnsEnumValueOfTargetType()
        {
            var mapped = EnumHelper.MapNullable<TestEnum2>(TestEnum1.Value2);

            Assert.True(mapped.HasValue);
            Assert.Equal(TestEnum2.Value2, mapped.Value);
        }

        [Fact]
        public void MapNullableWithNullSourceReturnsNull()
        {
            var result = EnumHelper.MapNullable<TestEnum1>(null);

            Assert.Null(result);
        }

        [Fact]
        public void MapNullableWithSourceNotOnTargetEnumThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => EnumHelper.MapNullable<TestEnum3>(TestEnum1.Value1));

            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void MapReturnsEnumValueOfTargetType()
        {
            var mapped = EnumHelper.Map<TestEnum2>(TestEnum1.Value2);

            Assert.Equal(TestEnum2.Value2, mapped);
        }

        [Fact]
        public void MapWithNullSourceThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => EnumHelper.Map<TestEnum2>(null));

            Assert.Equal("source", ex.ParamName);
        }

        [Fact]
        public void MapWithSourceNotOnTargetEnumThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => EnumHelper.Map<TestEnum3>(TestEnum1.Value1));

            Assert.Equal("source", ex.ParamName);
        }
    }
}