using Messanger.Dal.Context;
using Messanger.Dal.Entities;
using Messanger.Dal.Repositories;
using Messanger.Dal.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Messanger.Bll.Helpers
{
    public static class ConfigurationServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IRepository<Room>, Repository<Room>>();
        }
    }
}
