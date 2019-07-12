using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using UserManagement.EFDataAccess.Handlers.CommandHandlers;

namespace UserManagement.Extensions
{
    public static class MediatRHandlers
    {
        private static List<Assembly> assemblies = new List<Assembly>();

        private static void AddHandler()
        {
            assemblies.Add(typeof(CreateUserHandler).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddMediator(this IServiceCollection service)
        {
            AddHandler();
            service.AddMediatR(assemblies);
            return service;
        }
    }
}
