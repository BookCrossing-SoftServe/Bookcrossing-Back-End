using Application.Dto.Comment;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookCommentInsertValidator : AbstractValidator<BookCommentInsertDto>
    {
        public BookCommentInsertValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text).Length(1, 256);
            RuleFor(x => x.BookId).GreaterThan(0);
            RuleFor(x => x.CommentOwnerId).GreaterThan(0);
        }
    }
}
