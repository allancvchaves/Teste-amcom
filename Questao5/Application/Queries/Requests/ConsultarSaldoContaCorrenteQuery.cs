using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultarSaldoContaCorrenteQuery : IRequest<SaldoContaCorrenteResponse>
    {
        public string IdContaCorrente { get; set; }
    }
}
