using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class LanguageValidator : AbstractValidator<LanguageDto>
    {
        public LanguageValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Name)
                .NotNull()
                .Length(1, 20)
                .Matches(@"^([a-zA-Z||а-щА-ЩЬьЮюЯяЇїІіЄєҐґыЫэЭ]+(?:'|-)?[a-zA-Z||а-щА-ЩЬьЮюЯяЇїІіЄєҐґыЫэЭ])+$");
        }
    }
}
