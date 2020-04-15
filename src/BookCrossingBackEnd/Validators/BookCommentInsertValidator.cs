using Application.Dto.Comment;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookCommentInsertValidator : AbstractValidator<BookCommentInsertDto>
    {
        public BookCommentInsertValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null).WithMessage("Should not be null");
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text).NotNull().Length(1, 256);
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
            RuleFor(x => x.CommentOwnerId).NotNull().GreaterThan(0);
        }
    }
}
