using Application.Dto.Comment.Book;
using FluentValidation;
using System.Linq;

namespace BookCrossingBackEnd.Validators.Comment.Book
{
    public class ChildInsertValidator : AbstractValidator<ChildInsertDto>
    {
        public ChildInsertValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null && collection.Count() > 0).WithMessage("Should not be null or empty.");
            RuleForEach(x => x.Ids).NotNull().Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text).NotNull().Length(1, 256);
            RuleFor(x => x.Text).Must(text =>text !=null && text.Trim(' ').Length > 1).WithMessage("Should not contain only white spaces.");
            RuleFor(x => x.OwnerId).NotNull().GreaterThan(0);
        }
    }
}
