using Application.Dtos.Requests;
using FluentValidation;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Application.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress().WithMessage("Invalid E-mail address");
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
        RuleFor(e => e.Password).NotEmpty();
    }
}