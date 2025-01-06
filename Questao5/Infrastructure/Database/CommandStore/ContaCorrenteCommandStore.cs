using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class ContaCorrenteCommandStore
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly string formatoDataISO8601 = "yyyy-MM-dd HH:mm:ss";

        public ContaCorrenteCommandStore(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> MovimentarAsync(MovimentacaoContaCorrente movimentacao)
        {
            string sql = $@"
                INSERT INTO movimento (idmovimento, idcontacorrente, valor, tipomovimento, datamovimento)
                VALUES (@IdMovimento, @IdContaCorrente, @Valor, '{movimentacao.TipoMovimento}', '{movimentacao.DataMovimento.ToString(formatoDataISO8601)}');
                SELECT last_insert_rowid();";

            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, movimentacao);
        }
    }
}
