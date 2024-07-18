namespace Tarefas.Domain.Pagination
{
    /// <summary>
    /// Define os parâmetros para pesquisa e filtragem de usuários.
    /// </summary>
    public class ParametersUsuario : ParametersOrdem
    {
        /// <summary>
        /// Lista de identificadores únicos (Lyceum IDs) para fins de filtragem de usuários.
        /// </summary>
        public List<Guid> LyceumIds { get; set; }

        /// <summary>
        /// Número funcional do usuário para fins de filtragem.
        /// </summary>
        public string Funcional { get; set; }
    }
}