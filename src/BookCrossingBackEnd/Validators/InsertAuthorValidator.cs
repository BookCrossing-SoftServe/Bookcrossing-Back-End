using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class InsertAuthorValidator : AbstractValidator<InsertAuthorDto>
    {
        public InsertAuthorValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.FirstName)
                .NotNull()
                .Length(2, 20)
                .Matches(@"^([a-zA-Z '-]+)$");
            RuleFor(x => x.LastName)
                .NotNull()
                .Length(2, 20)
                .Matches(@"^([a-zA-Z '-]+)$");
            RuleFor(x => x.MiddleName)
                .Length(0, 30)
                .Matches(@"^([a-zA-Z '-]+)$");
        }
    }
}