using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookValidator: AbstractValidator<BookPutDto>
    {
        public BookValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleForEach(b => b.Authors).ChildRules(author => author.RuleFor(a => a.Id).GreaterThan(0).NotEmpty());
            RuleForEach(b => b.Genres).ChildRules(genre => genre.RuleFor(a => a.Id).GreaterThan(0).NotEmpty());
            RuleFor(b => b.Name)
                .Matches(@"^[A-Za-z0-9\s\-_,\.;:()?!#]+$")
                .Length(2, 50)
                .NotNull();
            RuleFor(b => b.UserId).NotNull();
            RuleFor(b => b.Publisher)
                .Matches(@"^[A-Za-z0-9\s\-_,\.;:()?!]+$")
                .MaximumLength(50);
        }
    }
}
