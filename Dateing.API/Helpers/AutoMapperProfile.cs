using System;
using System.Linq;
using AutoMapper;
using Dateing.API.DTO;
using Dateing.API.Models;

namespace Dateing.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserListModel>()
            .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(mf => mf.Photos.FirstOrDefault(p => p.IsMain).URL))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(mf => mf.BirthDay.CalculateAge()));
            CreateMap<User, UserDetailModel>()
            .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(mf => mf.Photos.FirstOrDefault(p => p.IsMain).URL))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(mf => mf.BirthDay.CalculateAge()));
            CreateMap<Photo, PhotoModel>();
            CreateMap<UserUpdateModel, User>();
            CreateMap<CreatePhotoModel, Photo>();
            CreateMap<Photo, ReturnPhotoModel>();
            CreateMap<RegisterUser, User>();
            CreateMap<CreateMessageModel, Message>().ReverseMap();
            CreateMap<Message, MessageToReturn>()
            .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(mf => mf.Sender.Photos.FirstOrDefault(p => p.IsMain).URL))
            .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(mf => mf.Recipient.Photos.FirstOrDefault(p => p.IsMain).URL));
        }
    }
}