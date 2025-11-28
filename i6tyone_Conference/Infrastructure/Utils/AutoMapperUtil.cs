using AutoMapper;
using eGhis_WebService_Core.Models.Db;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public class AutoMapperUtil : Profile
    {
        public AutoMapperUtil()
        {
            CreateMap<IC26DataRecord, RegisterInfo>();
            CreateMap<IC26DataRecord, CheckInResponseDto>();
            CreateMap<IC26DataRecord, RegisterInfoResponseDto>();
        }
    }
}
