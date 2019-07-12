using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain;
using UserManagement.Models;

namespace UserManagement.Configurations.Automapper.ProfileAutomapper
{
    public class ViewModelToDomain:Profile
    {
        public ViewModelToDomain()
        {
            CreateMap<UserViewModel, User>().ReverseMap();
        }
    }
}
