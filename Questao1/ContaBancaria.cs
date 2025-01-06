namespace Questao1
{
    class ContaBancaria
    {
        public int Numero { get; private set; }
        public string Titular { get; set; }
        private double saldo;

        public double Saldo
        {
            get { return saldo; }
        }

        public ContaBancaria(int numero, string titular, double depositoInicial = 0.0)
        {
            Numero = numero;
            Titular = titular;
            saldo = depositoInicial;
        }

        public void Deposito(double quantia)
        {
            if (quantia > 0)
            {
                saldo += quantia;
            }
        }

        public void Saque(double quantia)
        {
            const double taxa = 3.50;
            saldo -= (quantia + taxa);
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: R$ {saldo:F2}";
        }
    }
}
