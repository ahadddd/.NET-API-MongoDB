using AutoMapper;
using MongoDB.Bson;
using WebAPIMongo.DTO;

namespace WebAPIMongo.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Driver, DriverDTO>();
            CreateMap<DriverDTO, Driver>();
                
        }
    }
}
