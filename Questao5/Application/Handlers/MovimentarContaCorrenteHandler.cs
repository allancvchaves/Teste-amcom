using System.Text.Json;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaCorrenteHandler : IRequestHandler<MovimentarContaCorrenteRequest, MovimentarContaCorrenteResponse>
    {
        private readonly ContaCorrenteCommandStore _commandStore;
        private readonly ContaCorrenteQueryStore _queryStore;
        private readonly IdempotenciaStore _idempotenciaStore;

        public MovimentarContaCorrenteHandler(
            ContaCorrenteCommandStore commandStore,
            ContaCorrenteQueryStore queryStore,
            IdempotenciaStore idempotenciaStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
            _idempotenciaStore = idempotenciaStore;
        }

        public async Task<MovimentarContaCorrenteResponse> Handle(MovimentarContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            var resultadoExistente = await _idempotenciaStore.ObterResultadoPorChaveIdempotenciaAsync(request.IdRequisicao.ToString());
            if (resultadoExistente != null)
            {
                return new MovimentarContaCorrenteResponse
                {
                    IdMovimentacao = resultadoExistente,
                    Mensagem = "Movimentação já processada anteriormente."
                };
            }

            var contaCorrente = await _queryStore.ObterContaCorrentePorIdAsync(request.IdContaCorrente.ToString());

            ValidarMovimentacao(contaCorrente, request);

            var movimentacao = new MovimentacaoContaCorrente
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = request.IdContaCorrente.ToString(),
                Valor = request.Valor,
                IdRequisicao = request.IdRequisicao,
                DataMovimento = DateTime.Now,
                TipoMovimento = ConverterRequestParaTipoMovimento(request.TipoMovimento)
            };

            var id = await _commandStore.MovimentarAsync(movimentacao);

            await _idempotenciaStore.SalvarResultadoIdempotenteAsync(request.IdRequisicao.ToString(),
                JsonSerializer.Serialize(request), id.ToString());

            return new MovimentarContaCorrenteResponse
            {
                IdMovimentacao = id.ToString(),
                Mensagem = "Movimentação realizada com sucesso."
            };
        }

        private void ValidarMovimentacao(ContaCorrente? contaCorrente, MovimentarContaCorrenteRequest request)
        {
            if (contaCorrente is null)
            {
                throw new BusinessException("Conta não encontrada", "INVALID_ACCOUNT");
            }

            if (!contaCorrente.Ativo)
            {
                throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");
            }

            if (request.Valor <= 0)
            {
                throw new BusinessException("Valor inválido", "INVALID_VALUE");
            }

            if (!request.TipoMovimento.Equals("C", StringComparison.OrdinalIgnoreCase)
                && !request.TipoMovimento.Equals("D", StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException("Tipo de movimentação inválido", "INVALID_TYPE");
            }
        }

        private TipoMovimento ConverterRequestParaTipoMovimento(string letraMovimento) => letraMovimento.Equals("C", StringComparison.OrdinalIgnoreCase) ? TipoMovimento.C : TipoMovimento.D;
    }
}
