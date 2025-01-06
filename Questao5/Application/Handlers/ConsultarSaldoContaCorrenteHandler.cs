using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoContaCorrenteHandler : IRequestHandler<ConsultarSaldoContaCorrenteQuery, SaldoContaCorrenteResponse>
    {
        private readonly ContaCorrenteQueryStore _queryStore;

        public ConsultarSaldoContaCorrenteHandler(ContaCorrenteQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<SaldoContaCorrenteResponse> Handle(ConsultarSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            // Validação da conta corrente
            var contaCorrente = await _queryStore.ObterContaCorrentePorIdAsync(request.IdContaCorrente.ToString());
            if (contaCorrente == null)
            {
                throw new BusinessException("Conta não encontrada", "INVALID_ACCOUNT");
            }

            if (!contaCorrente.Ativo)
            {
                throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");
            }

            // Obter movimentações da conta
            var saldo = await _queryStore.CalcularSaldoPorContaAsync(request.IdContaCorrente.ToString());

            // Retornar resposta com dados completos
            return new SaldoContaCorrenteResponse
            {
                NumeroContaCorrente = contaCorrente.Numero,
                NomeTitular = contaCorrente.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}
