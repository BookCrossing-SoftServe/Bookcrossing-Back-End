using Application.Dto.Comment;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookCommentUpdateValidator : AbstractValidator<BookCommentUpdateDto>
    {
        public BookCommentUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text).Length(1, 256);
        }
    }
}
