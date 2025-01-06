namespace Questao5.Application.Queries.Responses
{
    public class SaldoContaCorrenteResponse
    {
        public int NumeroContaCorrente { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal Saldo { get; set; }
    }

}