namespace CadastroFuncionarios
{
    public class AuthService
    {
        private readonly string usuario = "admin";
        private readonly string senha = "1234";

        public bool Login(string user, string pass)
        {
            return user == usuario && pass == senha;
        }
    }
}
