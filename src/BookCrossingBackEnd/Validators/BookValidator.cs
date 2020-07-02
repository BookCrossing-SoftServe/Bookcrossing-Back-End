using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookValidator: AbstractValidator<BookPostDto>
    {
        public BookValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleForEach(b => b.Genres).ChildRules(author => author.RuleFor(a => a.Id).GreaterThan(0).NotEmpty());
            RuleForEach(b => b.Authors).ChildRules(genre => genre.RuleFor(a => a.Id).GreaterThan(0).NotEmpty());
            RuleFor(b => b.Name)
                .Length(1, 100)
                .NotNull();
            RuleFor(b => b.UserId).NotNull();
            RuleFor(b => b.Publisher)
                .Length(1,100);
        }
    }
}
