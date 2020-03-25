﻿using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(x => x.Id).GreaterThanOrEqualTo(0);
            RuleFor(x => x.FirstName).Length(2, 20);
            RuleFor(x => x.LastName).Length(2, 20);
            RuleFor(x => x.MiddleName).Length(0, 30);
        }
    }
}
