using Asp.Versioning.ApiExplorer;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Tarefas.API.Configuration;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Configura��o do Serilog para logging
    SerilogConfig.AddSerilogApi();
    builder.Host.UseSerilog(Log.Logger);
    Log.Warning("Iniciando API");

    // Configura��es e ambiente da aplica��o
    ConfigurationManager configurationManager = builder.Configuration;
    IWebHostEnvironment environment = builder.Environment;
    Log.Warning("Ambiente atual: {EnvironmentName}", environment.EnvironmentName);

    // Registro de servi�os
    builder.Services.AddControllers();
    builder.Services.AddJwtConfiguration(configurationManager);
    builder.Services.AddFluentValidationConfiguration();
    builder.Services.AddAutoMapperConfiguration();
    builder.Services.AddDatabaseConfiguration(configurationManager);
    builder.Services.AddDependencyInjectionConfiguration();
    builder.Services.AddSMTPConfiguration(configurationManager);
    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddCorsConfiguration();
    builder.Services.AddVersionConfiguration();
    builder.Services.AddHealthChecksConfiguration(configurationManager);
    builder.Services.AddHangfireConfiguration(builder.Configuration);
    builder.Services.AddLyceumAuthenticationClient(builder.Configuration);
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Constru��o da aplica��o Web
    WebApplication app = builder.Build();
    Log.Warning("Aplica��o constru�da.");
    IApiVersionDescriptionProvider provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configura��o do pipeline de requisi��es HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseCors("Development");
    }
    else
    {
        app.UseCors(app.Environment.IsStaging() ? "Staging" : "Production");

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
    }

    // Configura��o do Dashboard do Hangfire
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter
            {
                 User = configurationManager.GetSection("HangfireSettings:UserName").Value,
                 Pass = configurationManager.GetSection("HangfireSettings:Password").Value
            }
        }
    });

    // Configura��es adicionais e mapeamento de rotas
    app.ConfigureRecurringJobs();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseJwtConfiguration();
    app.UseDatabaseConfiguration();
    app.UseSwaggerConfiguration(environment, provider);
    app.UseHealthChecksConfiguration();
    app.MapControllers();

    // Iniciando a aplica��o
    Log.Warning("API iniciada. Aguardando requisi��es...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uma exce��o n�o tratada {ExceptionType} ocorreu, levando ao t�rmino da API. Detalhes: {ExceptionMessage}, Pilha de Chamadas: {StackTrace}", ex.GetType().Name, ex.Message, ex.StackTrace);
}
finally
{
    Log.Information("A execu��o da API foi conclu�da �s {DateTime}. Fechando e liberando recursos...", DateTime.Now);
    Log.CloseAndFlush();
}