using Serilog;

namespace Tarefas.API.Configuration
{
    public static class HttpClientConfig
    {
        public static void AddLyceumAuthenticationClient(this IServiceCollection services, IConfiguration configuration)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string baseUrlKey = environment == "Development" ? "AuthenticationClient:DevelopmentBaseUrl" : "AuthenticationClient:ProductionBaseUrl";

            string baseUrl = configuration[baseUrlKey];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                Log.Error("URL base para AuthenticationClient não está configurado para o ambiente: {Environment}.", environment);
                return;
            }

            services.AddHttpClient("AuthenticationClient", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });
        }
    }
}