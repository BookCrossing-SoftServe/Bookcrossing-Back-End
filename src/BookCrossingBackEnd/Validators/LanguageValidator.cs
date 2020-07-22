using System.Data;
using Application.Dto;
using FluentValidation;
using MimeKit.Encodings;

namespace BookCrossingBackEnd.Validators
{
    public class LanguageValidator : AbstractValidator<LanguageDto>
    {
        public LanguageValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name)
                .Length(1, 20)
                .Matches(@"^[a-zA-Z||а-щА-ЩЬьЮюЯяЇїІіЄєҐґыЫэЭ]+$");
        }
    }
}
