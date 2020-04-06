using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.FirstName).NameValidator(2, 20);
            RuleFor(x => x.LastName).NameValidator(2, 20);
            RuleFor(x => x.MiddleName).NameValidator(2, 20);
        }
    }
}
