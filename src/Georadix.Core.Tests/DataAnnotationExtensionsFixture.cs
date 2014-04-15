namespace Georadix.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class DataAnnotationExtensionsFixture
    {
        [Fact]
        public void AssertValidOnValidObjectDoesNotThrowException()
        {
            var sut = new TestModel() { Name = "Name", Title = string.Empty };

            sut.AssertValid("sut");
        }

        [Fact]
        public void AssertValidOnInvalidObjectThrowsArgumentException()
        {
            var sut = new TestModel();

            var ex = Assert.Throws<ArgumentException>(() => sut.AssertValid("sut"));

            Assert.Equal("sut", ex.ParamName);
            Assert.Contains("Name: Value is required.", ex.Message);
            Assert.Contains("Title: Value is required.", ex.Message);
        }

        [Fact]
        public void ValidateInvalidObjectReturnsListValidationResults()
        {
            var sut = new TestModel() { Title = "Title" };

            var validationResults = sut.Validate();

            Assert.NotEmpty(validationResults);

            var result = validationResults.First();

            Assert.Equal(new string[] { "Name" }, result.MemberNames);
            Assert.Equal("Value is required.", result.ErrorMessage);
        }

        [Fact]
        public void ValidateValidObjectReturnsEmptyListValidationResult()
        {
            var sut = new TestModel() { Name = "Name", Title = "Title" };

            var validationResults = sut.Validate();

            Assert.Empty(validationResults);
        }

        private class TestModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
            public string Name { get; set; }

            [Required(AllowEmptyStrings = true, ErrorMessage = "Value is required.")]
            public string Title { get; set; }
        }
    }
}