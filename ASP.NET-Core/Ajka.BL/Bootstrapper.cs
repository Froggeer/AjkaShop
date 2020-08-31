using System;
using System.Linq;
using System.Reflection;
using Ajka.BL.Facades.Base;
using Ajka.BL.Facades.Category;
using Ajka.BL.Facades.Invoice;
using Ajka.BL.Facades.ItemCard;
using Ajka.BL.Facades.User;
using Ajka.BL.Models.Base;
using Ajka.BL.Models.Category;
using Ajka.BL.Models.Invoice;
using Ajka.BL.Models.ItemCard;
using Ajka.BL.Models.User;
using Ajka.BL.Services.Base.Interfaces;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Ajka.BL
{
    public class Bootstrapper
    {
        public void RegisterServices(IServiceCollection services)
        {
            var ajkaServices = GetType().Assembly.GetTypes().Where(m => m.IsClass && m.GetInterface(nameof(IAjkaShopService)) != null);
            foreach (var service in ajkaServices)
            {
                var serviceInterface = Type.GetType(service.GetInterfaces().SingleOrDefault(x => x.Name.Contains(service.Name))?.FullName ?? throw new InvalidOperationException());
                var myTypeService = Type.GetType(service.FullName ?? throw new InvalidOperationException());
                if (serviceInterface == null)
                {
                    continue;
                }
                services.AddTransient(serviceInterface, myTypeService);
            }

            RegisterCrudFacades(services);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static void RegisterCrudFacades(IServiceCollection services)
        {
            services.AddTransient<IEntityDtoFacade<UserDto, int>, UserCrudFacade>();
            services.AddTransient<IEntityDtoFacade<CategoryDto, int>, CategoryCrudFacade>();
            services.AddTransient<IEntityDtoFacade<ItemCardDto, int>, ItemCardCrudFacade>();
            services.AddTransient<IEntityDtoFacade<InvoiceDto, int>, InvoiceCrudFacade>();
            services.AddTransient<IEntityDtoFacade<InvoiceItemDto, int>, InvoiceItemCrudFacade>();
            services.AddTransient<IEntityDtoFacade<IndividualVariableDto, int>, IndividualVariableCrudFacade>();
        }
    }
}
