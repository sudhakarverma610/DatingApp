using AutoMapper;
using DatingApp.Api.Dto;
using DatingApp.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserListDto>().
                ForMember(dest => dest.PhotoUrl, otp =>
                otp.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url)).
                ForMember(dest => dest.Age, otp =>
                               otp.MapFrom(src => src.DateOfBirth.CalculateAge()));
                
            CreateMap<User, UserDetailDto>().
                ForMember(dest => dest.PhotoUrl, otp =>
                otp.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url)).
                ForMember(dest => dest.Age, otp =>
                               otp.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();

        }
    }
}
