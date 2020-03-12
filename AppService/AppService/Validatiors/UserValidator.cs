using AppService.Models;
using Domain.Model;
using FluentValidation;

namespace AppService.Validatiors
{
    
    public class UserValidator:AbstractValidator<CreateUserModel>
    {
        public UserValidator()
        {
            RuleFor(u => u.Password).MinimumLength(8);
            RuleFor(u => u.Role).NotNull().IsInEnum();
            RuleFor(u => u.Username).NotEmpty().MinimumLength(3);
        }
    }
}