using Application.Dto.Comment;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class BookCommentDeleteValidator : AbstractValidator<BookCommentDeleteDto>
    {
        public BookCommentDeleteValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
        }
    }
}
