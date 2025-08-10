using App.Core.App.Users.Command;
using Microsoft.Extensions.DependencyInjection;

namespace App.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(mfg =>
            {
                mfg.RegisterServicesFromAssemblyContaining<CreateUserDtoCommand>();
            });

            return services;
        }
    }
}
