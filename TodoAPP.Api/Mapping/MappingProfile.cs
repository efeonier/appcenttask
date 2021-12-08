using AutoMapper;
using TodoAPP.Api.Model.Request;
using TodoAPP.Api.Model.Response;
using TodoAPP.Core.Entity;
using TodoAPP.Infrastructure.Common;

namespace TodoAPP.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(ICryptoHelper cryptoHelper)
        {
            // viewmodel -> entity
            CreateMap<NewUserRequestModel, User>().ForMember(dest => dest.Password,
                src => src.MapFrom(x => cryptoHelper.Hash(x.Password)));

            CreateMap<TodoTaskResponseModel, TaskItem>().ReverseMap();
            CreateMap<TodoTaskCreateRequestModel, TaskItem>().ReverseMap();

            CreateMap<ToDoListItem, TodoResponseModel>().ReverseMap();
            CreateMap<ToDoListItem, TodoCreateRequestModel>().ReverseMap();

            // entity -> viewmodel
            CreateMap<User, UserResponseModel>().ReverseMap();
        }
    }
}