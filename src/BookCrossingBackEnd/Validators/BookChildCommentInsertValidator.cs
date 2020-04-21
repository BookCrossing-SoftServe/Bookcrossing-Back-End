using Application.Dto.Comment.Book;
using FluentValidation;
using System.Linq;

namespace BookCrossingBackEnd.Validators
{
    public class BookChildCommentInsertValidator : AbstractValidator<ChildInsertDto>
    {
        public BookChildCommentInsertValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null && collection.Count() > 0).WithMessage("Should not be null or empty.");
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text.Trim(' ')).NotNull().Length(1, 256);
            RuleFor(x => x.CommentOwnerId).NotNull().GreaterThan(0);
        }
    }
}
