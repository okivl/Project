using FluentValidation;
using Project.Core.Models.CreateUpdate;

namespace Project.Core.Validation
{
    public class UserValidator : AbstractValidator<BaseUser>
    {
        public UserValidator()
        {
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Поле пароля не заполнено")
                .MinimumLength(6).WithMessage("Длина проля должна быть не менее 6 символов")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Matches("[^a-zA-Z0-9]").WithMessage("Пароль должен содержать хотя бы один не алфавитно-цифровой символ")
                .Must(UpperCase).WithMessage("Пароль должен содержать хотя бы одну заглавную букву");
        }

        private bool UpperCase(string password)
        {
            return password.Any(char.IsUpper);
        }
    }
}
