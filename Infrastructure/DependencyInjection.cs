using App.Core.Interface;
using Infrastructure.context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using App.Core.Interface.IRepositories;
using Infrastructure.Repository;
using App.Core.Interfaces;
using Infrastructure.Services;
using App.Core.Interface.IServices;
using Infrastructure.Service;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this  IServiceCollection services,
                IConfiguration configuration)
        {
            // db
            services.AddScoped<IAppDbContext, AppDbContext>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<IJwtService, JwtService>();


            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IMessageService, MessageService>();


            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            return services;
        }
    }
}
