using Application.Dto.Comment.Book;
using FluentValidation;

namespace BookCrossingBackEnd.Validators.Comment.Book
{
    public class RootDeleteValidator : AbstractValidator<RootDeleteDto>
    {
        public RootDeleteValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).NotNull().Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.OwnerId).NotNull().GreaterThan(0);
        }
    }
}
