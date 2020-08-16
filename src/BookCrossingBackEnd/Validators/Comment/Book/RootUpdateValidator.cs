using Application.Dto.Comment.Book;
using FluentValidation;

namespace BookCrossingBackEnd.Validators.Comment.Book
{
    public class RootUpdateValidator : AbstractValidator<RootUpdateDto>
    {
        public RootUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).NotNull().Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text).NotNull().Length(1, 500);
            RuleFor(x => x.Text).Must(text => text != null && text.Trim(' ').Length >= 1).WithMessage("Should not contain only white spaces.");
            RuleFor(x => x.OwnerId).NotNull().GreaterThan(0);
        }
    }
}
