using Application.Dto.Comment.Book;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookRootCommentUpdateValidator : AbstractValidator<RootUpdateDto>
    {
        public BookRootCommentUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text.Trim(' ')).NotNull().Length(1, 256);
        }
    }
}
