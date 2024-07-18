using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tarefas.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            #region Serviços Scoped

            // Usuários
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUsuarioApplication, UsuarioApplication>();

            // Permissões
            services.AddScoped<IPermissaoRepository, PermissaoRepository>();
            services.AddScoped<IPermissaoService, PermissaoService>();
            services.AddScoped<IPermissaoApplication, PermissaoApplication>();

            // Usuario
            services.AddScoped<IUser, AspNetUser>();

            // Email
            services.AddScoped<IEmailApplication, EmailApplication>();

            #endregion Serviços Scoped

            #region Serviços Singleton

            // Token
            services.AddSingleton<TokenValidationService>();

            // Acesso ao contexto HTTP
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<EnviromentApplication>();

            #endregion Serviços Singleton

            #region Serviços Transient

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            #endregion Serviços Transient
        }
    }
}