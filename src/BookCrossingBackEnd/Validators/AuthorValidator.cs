using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FirstName)
                .NotNull()
                .Length(2, 20)
                .Matches(@"^([a-zA-Z '-]+)$");
            RuleFor(x => x.LastName)
                .NotNull()
                .Length(2, 20)
                .Matches(@"^([a-zA-Z '-]+)$");
            RuleFor(x => x.MiddleName)
                .MaximumLength(30)
                .Matches(@"&^|^([a-zA-Z '-]+)$")
                .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));
        }
    }
}
