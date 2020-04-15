using Application.Dto.Comment;
using FluentValidation;
using System.Linq;

namespace BookCrossingBackEnd.Validators
{
    public class BookCommentUpdateValidator : AbstractValidator<BookCommentUpdateDto>
    {
        public BookCommentUpdateValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null && collection.Count() > 0).WithMessage("Should not be null or empty");
            RuleForEach(x => x.Ids).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.Text).NotNull().Length(1, 256);
        }
    }
}
