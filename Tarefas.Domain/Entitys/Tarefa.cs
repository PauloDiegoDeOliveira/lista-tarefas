using Tarefas.Domain.Entitys.Base;

namespace Tarefas.Domain.Entitys
{
    public class Tarefa : EntityBase
    {
        public string Valor { get; set; }
        public bool Verificada { get; set; }

        public override EntityBase VerificarRelacionamentoParaRemocao()
        {
            return null;
        }
    }
}