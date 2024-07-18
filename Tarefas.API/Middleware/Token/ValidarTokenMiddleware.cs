using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Tarefas.API.Middleware.Token
{
    public class ValidarTokenMiddleware(RequestDelegate next,
                                         IServiceScopeFactory serviceScopeFactory,
                                         TokenValidationService tokenValidationService)
    {
        private readonly RequestDelegate next = next;
        private readonly TokenValidationService tokenValidationService = tokenValidationService;
        private readonly IServiceScopeFactory serviceScopeFactory = serviceScopeFactory;

        public async Task Invoke(HttpContext context)
        {
            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            IUsuarioApplication usuarioApplication = serviceScope.ServiceProvider.GetRequiredService<IUsuarioApplication>();
            INotifier notifier = serviceScope.ServiceProvider.GetRequiredService<INotifier>();

            if (await ProcessarAutorizacao(context, usuarioApplication, notifier))
            {
                await next(context);
            }

            await TratarRespostaProibida(context, notifier);
        }

        private async Task<bool> ProcessarAutorizacao(HttpContext context, IUsuarioApplication usuarioApplication, INotifier notifier)
        {
            if (context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() == null)
            {
                return true;
            }

            string token = ExtrairTokenDoCabecalho(context.Request.Headers.Authorization);
            if (string.IsNullOrEmpty(token))
            {
                AdicionarNotificacaoEResponder(notifier, context, "Autenticação necessária.", StatusCodes.Status401Unauthorized);
                return false;
            }

            AutenticacaoRespostaDto objToken = await tokenValidationService.ValidarTokenEObterInfoUsuario(token);
            if (objToken == null)
            {
                // Token inválido ou expirado.
                AdicionarNotificacaoEResponder(notifier, context, "Sua sessão expirou. Por favor, faça login novamente.", StatusCodes.Status401Unauthorized);
                return false;
            }

            ViewUsuarioDto usuarioPermissao = await usuarioApplication.GetByMiddlewareUsuarioIdAsync(objToken.Dados.UsuarioIdLyceum);
            if (usuarioPermissao == null)
            {
                AdicionarNotificacaoEResponder(notifier, context, "Usuário não cadastrado.", StatusCodes.Status403Forbidden);
                return false;
            }

            // Não permitir autenticação simultânea
            if (!ValidarVersaoToken(token, usuarioPermissao, notifier, context))
            {
                return false;
            }

            DefinirClaimsUsuario(context, objToken, usuarioPermissao);
            return true;
        }

        private string ExtrairTokenDoCabecalho(string authorizationHeader)
        {
            return authorizationHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ?? false
                ? authorizationHeader["Bearer ".Length..].Trim()
                : null;
        }

        private void DefinirClaimsUsuario(HttpContext context, AutenticacaoRespostaDto objToken, ViewUsuarioDto usuarioPermissao)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, objToken.Dados.UsuarioIdLyceum.ToString()),
                new(ClaimTypes.Email, objToken.Dados.Email.ToString()),
                new(ClaimTypes.Role, usuarioPermissao.Permissao.Nome.ToString()),
            ];

            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
        }

        private bool ValidarVersaoToken(string token, ViewUsuarioDto usuarioPermissao, INotifier notifier, HttpContext context)
        {
            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            string versaoTokenLisaClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "VersaoTokenLisa")?.Value;

            if (versaoTokenLisaClaim != null && versaoTokenLisaClaim != usuarioPermissao.VersaoToken.ToString())
            {
                AdicionarNotificacaoEResponder(notifier, context, "Sua conta foi acessada de outro dispositivo. A sessão expirou.", StatusCodes.Status401Unauthorized);
                return false;
            }

            return true;
        }

        private async Task TratarRespostaProibida(HttpContext context, INotifier notifier)
        {
            // Verifica se o status é 403 Forbidden e a resposta ainda não foi iniciada
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden && !context.Response.HasStarted)
            {
                string mensagemErro = "Acesso proibido: você não tem permissão para acessar este recurso.";

                // Verifica se há uma mensagem de erro específica no contexto relacionada ao horário comercial
                if (context.Items.ContainsKey("AuthorizationError"))
                {
                    mensagemErro = context.Items["AuthorizationError"].ToString();
                }

                notifier.AddNotification(new Notification(mensagemErro));
                await EscreverRespostaErroAsync(context, notifier, context.Response.StatusCode, mensagemErro);
            }
        }

        private void AdicionarNotificacaoEResponder(INotifier notifier, HttpContext context, string message, int statusCode)
        {
            notifier.AddNotification(new Notification(message));
            EscreverRespostaErroAsync(context, notifier, statusCode, message).Wait();
        }

        private async Task EscreverRespostaErroAsync(HttpContext context, INotifier notifier, int statusCode, string errorMessage)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                sucesso = false,
                erros = notifier.GetAllNotifications().Select(n => n.Message)
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}