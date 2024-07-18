using Hangfire;
using Hangfire.SqlServer;

namespace Tarefas.API.Configuration
{
    public static class HangfireConfig
    {
        public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    //.UseMemoryStorage());
                    .UseSqlServerStorage(configuration.GetConnectionString("Connection"), new SqlServerStorageOptions
                    {
                        TryAutoDetectSchemaDependentOptions = false
                    }));

            services.AddHangfireServer();
        }
    }
}