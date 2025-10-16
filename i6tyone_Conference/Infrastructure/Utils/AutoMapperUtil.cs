using AutoMapper;
using eGhis_WebService_Core.Models.Db;
using i6tyone_Conference.Models.Dto.Auth;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public class AutoMapperUtil : Profile
    {
        public AutoMapperUtil()
        {
            CreateMap<IC26DataRecord, RegisterInfo>();
        }
    }
}
