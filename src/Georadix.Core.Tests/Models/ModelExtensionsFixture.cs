namespace Georadix.Core.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Xunit;

    public class ModelExtensionsFixture
    {
        [Fact]
        public void AssertValidOnValidModelDoesNotThrowException()
        {
            var sut = new TestModel() { Name = "Name", Title = string.Empty };

            sut.AssertValid("sut");
        }

        [Fact]
        public void AssertValidOnInvalidModelThrowsArgumentException()
        {
            var sut = new TestModel() { };

            var ex = Assert.Throws<ArgumentException>(() => sut.AssertValid("sut"));

            Assert.Equal("sut", ex.ParamName);
            Assert.Contains("Name: Value is required.", ex.Message);
            Assert.Contains("Title: Value is required.", ex.Message);
        }

        private class TestModel : Model
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
            public string Name { get; set; }

            [Required(AllowEmptyStrings = true, ErrorMessage = "Value is required.")]
            public string Title { get; set; }
        }
    }
}
