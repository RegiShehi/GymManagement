﻿using GymManagement.Application.Gyms.Commands.CreateGym;
using MediatR;
using ErrorOr;
using GymManagement.Domain.Gyms;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            options.AddBehavior<IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>, CreateGymCommandBehaviour>();
        });

        return services;
    }
}