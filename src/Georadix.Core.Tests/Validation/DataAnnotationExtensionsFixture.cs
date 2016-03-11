namespace Georadix.Core.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class DataAnnotationExtensionsFixture
    {
        public static IEnumerable<object[]> InvalidModelScenarios
        {
            get
            {
                return new object[][]
                {
                    new object[]
                    {
                        new TestModel { Name = null },
                        new string[] { "Name" },
                        "Value is required."
                    },
                    new object[]
                    {
                        new TestModel { Name = "Name", SubModel = new SubModel() },
                        new string[] { "SubModel.Property1" },
                        "Value is required."
                    },
                    new object[]
                    {
                        new TestModel { Name = "Name", Collection = new SubModel[] { new SubModel() } },
                        new string[] { "Collection[0].Property1" },
                        "Value is required."
                    }
                };
            }
        }

        [Fact]
        public void AssertValidOnInvalidObjectThrowsArgumentException()
        {
            var sut = new TestModel();

            var ex = Assert.Throws<ArgumentException>(() => sut.AssertValid("sut"));

            Assert.Equal("sut", ex.ParamName);
            Assert.Contains("Name: Value is required.", ex.Message);
        }

        [Fact]
        public void AssertValidOnValidObjectDoesNotThrowException()
        {
            var sut = new TestModel
            {
                Name = "Name",
                SubModel = new SubModel
                {
                    Property1 = "1"
                }
            };

            sut.AssertValid("sut");
        }

        [Theory]
        [MemberData("InvalidModelScenarios")]
        public void ValidateInvalidObjectReturnsListValidationResults(
            TestModel sut, string[] memberNames, string errorMessage)
        {
            var validationResults = sut.Validate();

            Assert.NotEmpty(validationResults);

            var result = validationResults.First();

            Assert.Equal(memberNames, result.MemberNames);
            Assert.Equal(errorMessage, result.ErrorMessage);
        }

        [Fact]
        public void ValidateValidObjectReturnsEmptyListValidationResult()
        {
            var sut = new TestModel() { Name = "Name", SubModel = new SubModel { Property1 = "1" } };

            var validationResults = sut.Validate();

            Assert.Empty(validationResults);
        }

        public class SubModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
            public string Property1 { get; set; }
        }

        public class TestModel
        {
            public IEnumerable<SubModel> Collection { get; set; }

            public string Description { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
            public string Name { get; set; }

            public SubModel SubModel { get; set; }
        }
    }
}