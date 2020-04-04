using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> NameValidator<T>(this IRuleBuilder<T, string> rule, int minLength, int maxLength)
        {
            return rule
                .NotNull()
                .Length(minLength, maxLength)
                .Matches(@"^([a-zA-Z '-]+)$");
        }
    }
}
