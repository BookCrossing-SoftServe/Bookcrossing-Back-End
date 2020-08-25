using Application.Dto.Comment.Book;
using FluentValidation;

namespace BookCrossingBackEnd.Validators.Comment.Book
{
    public class RootInsertValidator : AbstractValidator<RootInsertDto>
    {
        public RootInsertValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Text).Must(text => text != null && text.Trim(' ').Length >= 1).WithMessage("Should not contain only white spaces.");
            RuleFor(x => x.Text).NotNull().Length(1, 500);
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
            RuleFor(x => x.OwnerId).NotNull().GreaterThan(0);
        }
    }
}
