using Application.Dto.Comment.Book;
using FluentValidation;
using System.Linq;

namespace BookCrossingBackEnd.Validators
{
    public class BookChildCommentUpdateValidator : AbstractValidator<ChildUpdateDto>
    {
        public BookChildCommentUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null && collection.Count() > 1).WithMessage("Should not be null and should have more than one element.");
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text.Trim(' ')).NotNull().Length(1, 256);
            RuleFor(x => x.CommentOwnerId).NotNull().GreaterThan(0);
        }
    }
}
