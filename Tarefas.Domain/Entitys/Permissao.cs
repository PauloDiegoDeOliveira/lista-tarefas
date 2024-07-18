using Tarefas.Domain.Entitys.Base;

namespace Tarefas.Domain.Entitys
{
    public class Permissao : EntityBase
    {
        public string Nome { get; set; }
        public string Observacao { get; set; }

        public virtual IList<Usuario> Usuarios { get; set; }

        public override EntityBase VerificarRelacionamentoParaRemocao()
        {
            return null;
        }
    }
}