using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using FluentValidation;

namespace BookCrossingBackEnd.Validators
{
    public class UserRoomValidator: AbstractValidator<RoomLocationDto>
    {
        public UserRoomValidator()
        {
            RuleFor(x => x.RoomNumber).NotNull().Length(1, 7).Matches(@"^[^\s]{1,7}$");
        }
    }
}
