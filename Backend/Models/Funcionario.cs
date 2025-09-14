using System;

namespace CadastroFuncionarios
{
    public class Funcionario
    {
        public int Id { get; set; } // chave primária no banco
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public string Raca { get; set; }
        public string PCD { get; set; }
        public string FormaPagamento { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
    }
}
