using Application.Dto;
using Domain.Entities;
using FluentValidation;

namespace Application.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FirstName).Length(2, 30);
            RuleFor(x => x.LastName).Length(2, 30);
            RuleFor(x => x.MiddleName).Length(0, 30);
        }
    }
}
