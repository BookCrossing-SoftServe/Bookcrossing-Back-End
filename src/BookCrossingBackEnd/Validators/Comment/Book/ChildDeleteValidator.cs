using Application.Dto.Comment.Book;
using FluentValidation;
using System.Linq;

namespace BookCrossingBackEnd.Validators.Comment.Book
{
    public class ChildDeleteValidator : AbstractValidator<ChildDeleteDto>
    {
        public ChildDeleteValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Ids).Must(collection => collection != null && collection.Count() > 1).WithMessage("Should not be null and should have more than one element.");
            RuleForEach(x => x.Ids).NotNull().Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.OwnerId).NotNull().GreaterThan(0);
        }
    }
}
