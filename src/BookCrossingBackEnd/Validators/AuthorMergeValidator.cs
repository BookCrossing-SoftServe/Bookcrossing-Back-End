using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class AuthorMergeValidator : AbstractValidator<AuthorMergeDto>
    {
        public AuthorMergeValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Author).SetValidator(new AuthorValidator());
            RuleFor(x => x.Authors).Must(ArrayMinLength);
        }

        private bool ArrayMinLength(int[] array)
        {
            return array.Length > 1;
        }
    }
}
