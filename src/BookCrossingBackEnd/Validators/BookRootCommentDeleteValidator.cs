using Application.Dto.Comment.Book;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCrossingBackEnd.Validators
{
    public class BookRootCommentDeleteValidator : AbstractValidator<RootDeleteDto>
    {
        public BookRootCommentDeleteValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Id).Matches(@"^[a-f\d]{24}$");
            RuleFor(x => x.CommentOwnerId).NotNull().GreaterThan(0);
        }
    }
}
