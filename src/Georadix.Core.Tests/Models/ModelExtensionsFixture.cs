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
            var sut = new TestModel() { Name = "Name", Title = "Title" };

            sut.AssertValid();
        }

        [Fact]
        public void AssertValidOnInvalidModelThrowsValidationException()
        {
            var sut = new TestModel() { Title = "Title" };

            var ex = Assert.Throws<ArgumentException>(() => sut.AssertValid());

            Assert.Equal("Properties: Name. Value is required.", ex.Message);
        }

        private class TestModel : Model
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
            public string Name { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Value is required.")]
            public string Title { get; set; }
        }
    }
}
