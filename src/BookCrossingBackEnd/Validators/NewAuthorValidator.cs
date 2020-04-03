using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class NewAuthorValidator : AbstractValidator<NewAuthorDto>
    {
        public NewAuthorValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.FirstName).NameValidator(2, 20);
            RuleFor(x => x.LastName).NameValidator(2, 20);
            RuleFor(x => x.MiddleName).NameValidator(0, 30);
        }
    }
}