using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCorrenteRequest : IRequest<MovimentarContaCorrenteResponse>
    {
        public Guid IdRequisicao { get; set; }
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } = string.Empty;
    }
}
