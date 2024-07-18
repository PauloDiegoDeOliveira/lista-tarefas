using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Net.Mime;

namespace Tarefas.API.Configuration
{
    public static class HealthChecksConfig
    {
        private static readonly string[] tags = ["db", "data"];

        public static void AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                 .AddSqlServer(configuration.GetConnectionString("Connection"),
                   name: "sqlserver", tags: tags);
        }

        public static void UseHealthChecksConfiguration(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/status",
                    new HealthCheckOptions()
                    {
                        ResponseWriter = async (context, report) =>
                        {
                            string result = JsonConvert.SerializeObject(
                                new
                                {
                                    horaAtual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                    statusAplicação = report.Status.ToString(),
                                    healthChecks = report.Entries.Select(e => new
                                    {
                                        check = e.Key,
                                        ErrorMessage = e.Value.Exception?.Message,
                                        status = Enum.GetName(typeof(HealthStatus), e.Value.Status),
                                        duration = e.Value.Duration.ToString()
                                    })
                                });
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(result);
                        }
                    })/*.RequireAuthorization()*/;
            });
        }
    }
}