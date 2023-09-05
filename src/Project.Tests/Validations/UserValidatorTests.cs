using FluentValidation;
using Project.Core.Models.CreateUpdate;
using Project.Core.Validation;
using Xunit;

namespace Project.UnitTests.Validations
{
    public class UserValidatorTests
    {
        private readonly UserRegParameters _userRegParameters;

        public UserValidatorTests()
        {
            _userRegParameters = new UserRegParameters()
            {
                FirstName = "Test",
                Surname = "Test",
                LastName = "Test",
                DateBirth = DateTime.Now,
                Email = "Test",
                Login = "Test",
                Password = "Test",
                PasswordConfirm = "Test",
            };
        }

        [Fact]
        public async Task UserRegistrationValidation_NotValidException()
        {
            // Arrange
            var validator = new UserValidator();

            //Act

            // Assert
            await Assert.ThrowsAsync<ValidationException>(() => validator.ValidateAndThrowAsync(_userRegParameters));
        }
    }
}
