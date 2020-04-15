using Application.Dto.Comment;
using FluentValidation;
using System.Linq;

namespace BookCrossingBackEnd.Validators
{
    public class BookCommentDeleteValidator : AbstractValidator<BookCommentDeleteDto>
    {
        public BookCommentDeleteValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null && collection.Count() > 0).WithMessage("Should not be null or empty");
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
        }
    }
}
