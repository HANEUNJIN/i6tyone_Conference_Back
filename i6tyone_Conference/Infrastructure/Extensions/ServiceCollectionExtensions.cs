using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using System.Reflection;

namespace eGhis_WebService_Core.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 지정된 어셈블리에서 IDaoMarker를 구현한 클래스들을 자동으로 스캔하여 DI 컨테이너에 등록합니다.
        /// </summary>
        public static void AddDaos(this IServiceCollection services, Assembly assembly)
        {
            var daoTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => typeof(IDaoMarker).IsAssignableFrom(i) && i != typeof(IDaoMarker)))
                .Select(t => new
                {
                    Implementation = t,
                    Interface = t.GetInterfaces().First(i => typeof(IDaoMarker).IsAssignableFrom(i) && i != typeof(IDaoMarker))
                });

            foreach (var dao in daoTypes)
            {
                services.AddScoped(dao.Interface, dao.Implementation);
            }
        }

        /// <summary>
        /// 지정된 어셈블리에서 IServiceMarker를 구현한 클래스들을 자동으로 스캔하여 DI 컨테이너에 등록합니다.
        /// </summary>
        public static void AddServices(this IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => typeof(IServiceMarker).IsAssignableFrom(i) && i != typeof(IServiceMarker)))
                .Select(t => new
                {
                    Implementation = t,
                    Interface = t.GetInterfaces().First(i => typeof(IServiceMarker).IsAssignableFrom(i) && i != typeof(IServiceMarker))
                });

            foreach (var service in serviceTypes)
            {
                services.AddScoped(service.Interface, service.Implementation);
            }
        }

        /// <summary>
        /// 지정된 어셈블리에서 IValidatorMaker를 구현한 클래스를 자동 등록합니다.
        /// </summary>
        //public static void AddValidators(this IServiceCollection services, Assembly assembly)
        //{
        //    var validatorTypes = assembly
        //        .GetTypes()
        //        .Where(t => !t.IsAbstract && !t.IsInterface)
        //        .SelectMany(t => t.GetInterfaces()
        //            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
        //            .Select(i => new { Interface = i, Implementation = t }));

        //    foreach (var v in validatorTypes)
        //    {
        //        services.AddScoped(v.Interface, v.Implementation);
        //    }
        //}
    }
}
