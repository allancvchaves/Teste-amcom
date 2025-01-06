using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class MovimentacaoContaCorrente
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
        public decimal Valor { get; set; }
        public Guid IdRequisicao { get; set; }
    }
}
