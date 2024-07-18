namespace Tarefas.API.Configuration
{
    public static class SmtpConfig
    {
        public static void AddSMTPConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
        }
    }
}