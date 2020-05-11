using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.User;
using SplitList.Models;
using Xamarin.Forms.Internals;
using Profile = AutoMapper.Profile;


namespace SplitList.AutoMapper
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>()
                .PreserveReferences();

            CreateMap<User, UserDTO>()
                .PreserveReferences();
        }
    }
}
