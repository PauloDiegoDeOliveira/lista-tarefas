using Tarefas.Domain.Enums;

namespace Tarefas.Domain.Pagination
{
    /// <summary>
    /// Define os parâmetros para ordenação em pesquisas e filtragens gerais.
    /// </summary>
    public class ParametersOrdem : ParametersBase
    {
        /// <summary>
        /// Define a ordenação pelo nome.
        /// </summary>
        public EOrdenar OrdenarNome { get; set; }

        /// <summary>
        /// Define a ordenação pela data de criação.
        /// </summary>
        public EOrdenar OrdenarCriadoEm { get; set; }
    }
}