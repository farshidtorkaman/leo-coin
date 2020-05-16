using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using Crypto.Application.Users.Documents.Commands;
using Crypto.Application.Users.Documents.Queries;
using Crypto.Application.Users.FinancialInformation.Commands;
using Crypto.Application.Users.FinancialInformation.Queries;
using Crypto.Application.Users.Profile.Commands;
using Crypto.Application.Users.Profile.Queries;

namespace Crypto.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

            CreateMap<UpdateProfileCommand, UserProfileVm>();
            CreateMap<UpdateFinancialInfoCommand, FinancialInfoVm>();
            CreateMap<UpdateDocumentCommand, DocumentVm>();
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping") 
                    ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");
                
                methodInfo?.Invoke(instance, new object[] { this });

            }
        }
    }
}