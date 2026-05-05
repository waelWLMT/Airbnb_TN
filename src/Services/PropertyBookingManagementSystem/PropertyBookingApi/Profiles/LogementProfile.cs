using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace PropertyBookingApi.Profiles
{
    public class LogementProfile : Profile
    {
        public LogementProfile()
        {
            CreateMap<Logement, LogementReadDto>();             
        }
    }
}
