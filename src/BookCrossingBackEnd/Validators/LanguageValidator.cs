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
            RuleFor(x => x.Name)
                .NotNull()
                .Length(1, 20)
                .Matches(@"а-яА-Яa-zA-Z0-9+$");
        }
    }
}
