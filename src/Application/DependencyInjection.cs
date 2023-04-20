using Application._Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}