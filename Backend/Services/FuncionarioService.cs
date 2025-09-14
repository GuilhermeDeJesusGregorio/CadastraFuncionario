using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CadastroFuncionarios
{
    public class FuncionarioService
    {
        private string connectionString = "server=localhost;database=cadastro_funcionarios;user=funcionario_app;password=senha123;CharSet=utf8mb4;";

        public void Add(Funcionario f)
        {
            if (string.IsNullOrWhiteSpace(f.Nome))
                throw new Exception("O campo Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(f.CPF))
                throw new Exception("O campo CPF é obrigatório.");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Verifica se CPF já existe
                string checkSql = "SELECT COUNT(*) FROM funcionarios WHERE CPF=@CPF";
                using (var cmdCheck = new MySqlCommand(checkSql, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@CPF", f.CPF);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                    if (count > 0)
                        throw new Exception("CPF já cadastrado.");
                }

                string sql = @"INSERT INTO funcionarios 
                               (Nome, CPF, DataNascimento, DataAdmissao, DataDemissao, Raca, PCD, FormaPagamento, Banco, Agencia, Conta) 
                               VALUES 
                               (@Nome, @CPF, @DataNascimento, @DataAdmissao, @DataDemissao, @Raca, @PCD, @FormaPagamento, @Banco, @Agencia, @Conta)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", f.Nome);
                    cmd.Parameters.AddWithValue("@CPF", f.CPF);
                    cmd.Parameters.AddWithValue("@DataNascimento", f.DataNascimento);
                    cmd.Parameters.AddWithValue("@DataAdmissao", f.DataAdmissao);
                    cmd.Parameters.AddWithValue("@DataDemissao", f.DataDemissao.HasValue ? (object)f.DataDemissao.Value : DBNull.Value);

                    cmd.Parameters.AddWithValue("@Raca", string.IsNullOrWhiteSpace(f.Raca) ? (object)DBNull.Value : f.Raca);
                    cmd.Parameters.AddWithValue("@PCD", string.IsNullOrWhiteSpace(f.PCD) ? (object)DBNull.Value : f.PCD);
                    cmd.Parameters.AddWithValue("@FormaPagamento", string.IsNullOrWhiteSpace(f.FormaPagamento) ? (object)DBNull.Value : f.FormaPagamento);
                    cmd.Parameters.AddWithValue("@Banco", string.IsNullOrWhiteSpace(f.Banco) ? (object)DBNull.Value : f.Banco);
                    cmd.Parameters.AddWithValue("@Agencia", string.IsNullOrWhiteSpace(f.Agencia) ? (object)DBNull.Value : f.Agencia);
                    cmd.Parameters.AddWithValue("@Conta", string.IsNullOrWhiteSpace(f.Conta) ? (object)DBNull.Value : f.Conta);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Funcionario f)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"UPDATE funcionarios SET 
                               Nome=@Nome, CPF=@CPF, DataNascimento=@DataNascimento, DataAdmissao=@DataAdmissao,
                               DataDemissao=@DataDemissao, Raca=@Raca, PCD=@PCD, FormaPagamento=@FormaPagamento, 
                               Banco=@Banco, Agencia=@Agencia, Conta=@Conta
                               WHERE Id=@Id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", f.Id);
                    cmd.Parameters.AddWithValue("@Nome", f.Nome);
                    cmd.Parameters.AddWithValue("@CPF", f.CPF);
                    cmd.Parameters.AddWithValue("@DataNascimento", f.DataNascimento);
                    cmd.Parameters.AddWithValue("@DataAdmissao", f.DataAdmissao);
                    cmd.Parameters.AddWithValue("@DataDemissao", f.DataDemissao.HasValue ? (object)f.DataDemissao.Value : DBNull.Value);

                    cmd.Parameters.AddWithValue("@Raca", string.IsNullOrWhiteSpace(f.Raca) ? (object)DBNull.Value : f.Raca);
                    cmd.Parameters.AddWithValue("@PCD", string.IsNullOrWhiteSpace(f.PCD) ? (object)DBNull.Value : f.PCD);
                    cmd.Parameters.AddWithValue("@FormaPagamento", string.IsNullOrWhiteSpace(f.FormaPagamento) ? (object)DBNull.Value : f.FormaPagamento);
                    cmd.Parameters.AddWithValue("@Banco", string.IsNullOrWhiteSpace(f.Banco) ? (object)DBNull.Value : f.Banco);
                    cmd.Parameters.AddWithValue("@Agencia", string.IsNullOrWhiteSpace(f.Agencia) ? (object)DBNull.Value : f.Agencia);
                    cmd.Parameters.AddWithValue("@Conta", string.IsNullOrWhiteSpace(f.Conta) ? (object)DBNull.Value : f.Conta);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Funcionario> GetAll()
        {
            var lista = new List<Funcionario>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM funcionarios";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Funcionario f = new Funcionario
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            Nome = reader["Nome"] != DBNull.Value ? reader["Nome"].ToString() : "",
                            CPF = reader["CPF"] != DBNull.Value ? reader["CPF"].ToString() : "",
                            DataNascimento = reader["DataNascimento"] != DBNull.Value ? Convert.ToDateTime(reader["DataNascimento"]) : DateTime.MinValue,
                            DataAdmissao = reader["DataAdmissao"] != DBNull.Value ? Convert.ToDateTime(reader["DataAdmissao"]) : DateTime.MinValue,
                            DataDemissao = reader["DataDemissao"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["DataDemissao"]) : null,
                            Raca = reader["Raca"] != DBNull.Value ? reader["Raca"].ToString() : "",
                            PCD = reader["PCD"] != DBNull.Value ? reader["PCD"].ToString() : "",
                            FormaPagamento = reader["FormaPagamento"] != DBNull.Value ? reader["FormaPagamento"].ToString() : "",
                            Banco = reader["Banco"] != DBNull.Value ? reader["Banco"].ToString() : "",
                            Agencia = reader["Agencia"] != DBNull.Value ? reader["Agencia"].ToString() : "",
                            Conta = reader["Conta"] != DBNull.Value ? reader["Conta"].ToString() : ""
                        };

                        lista.Add(f);
                    }
                }
            }

            return lista;
        }

        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM funcionarios WHERE Id=@Id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int affected = cmd.ExecuteNonQuery();

                    if (affected == 0)
                        throw new Exception("Nenhum funcionário encontrado com este Id.");
                }
            }
        }
    }
}
