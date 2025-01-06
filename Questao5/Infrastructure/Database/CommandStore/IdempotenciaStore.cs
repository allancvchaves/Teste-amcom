using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class IdempotenciaStore
    {
        private readonly DatabaseConfig _databaseConfig;

        public IdempotenciaStore(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<string?> ObterResultadoPorChaveIdempotenciaAsync(string chaveIdempotencia)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            var resultado = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @Chave",
                new { Chave = chaveIdempotencia });

            return resultado;
        }

        public async Task SalvarResultadoIdempotenteAsync(string chaveIdempotencia, string requisicao, string resultado)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.ExecuteAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@Chave, @Requisicao, @Resultado)",
                new { Chave = chaveIdempotencia, Requisicao = requisicao, Resultado = resultado });
        }
    }
}
