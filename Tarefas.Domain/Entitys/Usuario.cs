using Tarefas.Domain.Entitys.Base;

namespace Tarefas.Domain.Entitys
{
    public class Usuario : EntityBase
    {
        public Guid UsuarioIdLyceum { get; set; }
        public Guid PermissaoId { get; set; }
        public long VersaoToken { get; set; }
        public bool NotificarPorEmail { get; set; }

        public virtual Permissao Permissao { get; set; }

        public override EntityBase VerificarRelacionamentoParaRemocao()
        {
            return null;
        }
    }
}