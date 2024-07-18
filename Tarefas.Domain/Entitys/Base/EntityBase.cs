namespace Tarefas.Domain.Entitys.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime? CriadoEm { get; set; } = DateTime.Now;
        public DateTime? AlteradoEm { get; set; }

        public EntityBase()
        { }

        public abstract EntityBase VerificarRelacionamentoParaRemocao();
    }
}