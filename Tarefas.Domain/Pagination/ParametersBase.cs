using Tarefas.Domain.Enums;

namespace Tarefas.Domain.Pagination
{
    /// <summary>
    /// Classe base para parâmetros de paginação, incluindo ID, status, e configurações de paginação.
    /// </summary>
    public class ParametersBase
    {
        private const int tamanhoMaximoResultados = 150;
        private int resultadosExibidos = 10;
        private int numeroPagina = 1;

        /// <summary>
        /// Identificadores para filtragem.
        /// </summary>
        public List<Guid> Id { get; set; }

        /// <summary>
        /// Status para filtragem.
        /// </summary>
        public EStatus Status { get; set; }

        /// <summary>
        /// Número da página para a pesquisa paginada.
        /// </summary>
        public int NumeroPagina
        {
            get => numeroPagina;
            set => numeroPagina = (value <= 0) ? numeroPagina : value;
        }

        /// <summary>
        /// Quantidade de resultados a serem exibidos por página.
        /// </summary>
        public int ResultadosExibidos
        {
            get => resultadosExibidos;
            set => resultadosExibidos = (value <= 0 || value > tamanhoMaximoResultados) ? resultadosExibidos : value;
        }
    }
}