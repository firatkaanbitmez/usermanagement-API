using AutoMapper;
using UserManagement.Core.DTOs;
using UserManagement.Core.Entities;

namespace UserManagement.Service.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
