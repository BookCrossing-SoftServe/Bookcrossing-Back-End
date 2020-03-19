using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using Domain.Entities;

namespace Application.IServices
{
    public interface IUserProfileService
    {
        /// <summary>
        /// Method return User Profile
        /// </summary>
        /// <param name="userId">Accepts User Id</param>
        /// <returns></returns>
        UserProfileDto GetMyProfile(int userId);
        /// <summary>
        /// Method add new Book to User Profile
        /// </summary>
        /// <param name="book">Book object from sent from form</param>
        void AddNewBook(Book book);
    }
}
