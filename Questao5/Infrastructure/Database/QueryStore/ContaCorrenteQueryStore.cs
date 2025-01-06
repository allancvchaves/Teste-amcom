using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.DTOs;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class ContaCorrenteQueryStore
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContaCorrenteQueryStore(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<ContaCorrente?> ObterContaCorrentePorIdAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id",
                new { Id = idContaCorrente });
        }

        public async Task<decimal> CalcularSaldoPorContaAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var resultado = await connection.QueryAsync<MovimentoDto>(
                "SELECT tipomovimento, valor FROM movimento WHERE idcontacorrente = @Id",
                new { Id = idContaCorrente });

            var saldo = resultado.Sum(m =>
                m.TipoMovimento.Equals("C", StringComparison.OrdinalIgnoreCase) ? m.Valor :
                m.TipoMovimento.Equals("D", StringComparison.OrdinalIgnoreCase) ? -m.Valor : 0);

            return saldo;
        }
    }
}
