using Application.Dto.Comment.Book;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookRootCommentInsertValidator : AbstractValidator<RootInsertDto>
    {
        public BookRootCommentInsertValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Text.Trim(' ')).NotNull().Length(1, 256);
            RuleFor(x => x.BookId).NotNull().GreaterThan(0);
            RuleFor(x => x.CommentOwnerId).NotNull().GreaterThan(0);
        }
    }
}
