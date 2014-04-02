namespace Georadix.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class ModelFixture
    {
        [Fact]
        public void ValidateInvalidModelReturnsListValidationResults()
        {
            var sut = new TestModel() { Title = "Title" };

            var validationResults = sut.Validate();

            Assert.NotEmpty(validationResults);

            var result = validationResults.First();

            Assert.Equal(new string[] { "Name" }, result.MemberNames);
            Assert.Equal("Name is required.", result.ErrorMessage);
        }

        [Fact]
        public void ValidateValidModelReturnsEmptyListValidationResult()
        {
            var sut = new TestModel() { Name = "Name", Title = "Title" };

            var validationResults = sut.Validate();

            Assert.Empty(validationResults);
        }

        private class TestModel : Model
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
            public string Name { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
            public string Title { get; set; }
        }
    }
}
