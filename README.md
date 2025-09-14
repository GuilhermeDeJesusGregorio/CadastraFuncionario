CadastraFuncionario

Sistema desktop em C# para cadastro e gerenciamento de funcionários, com persistência de dados em MySQL. O projeto permite armazenar, visualizar e gerenciar informações de colaboradores de forma prática e eficiente.

📝 Funcionalidades

Cadastro de funcionários com informações como:
Nome
Cargo
Salário
Data de admissão
Cor/Raça
Forma de pagamento 
Instituição bancária
Visualização de registros cadastrados.

Conexão com MySQL para armazenamento permanente.

Interface gráfica amigável utilizando Windows Forms.

Estrutura organizada em Frontend (interface) e Backend (lógica e banco).

💻 Tecnologias Utilizadas

C# (.NET Framework / Windows Forms)
MySQL para banco de dados
MySQL Connector/NET para integração C# ↔ MySQL
Visual Studio como ambiente de desenvolvimento

🛠 Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:
Visual Studio
MySQL Server
MySQL Workbench (opcional, para gerenciar o banco visualmente)
MySQL Connector/NET

🚀 Como Usar
Clone o repositório:
git clone https://github.com/GuilhermeDeJesusGregorio/CadastraFuncionario.git
Abra o projeto no Visual Studio.
Configure o banco de dados MySQL:

CREATE DATABASE CadastraFuncionarioDB;
USE CadastraFuncionarioDB;

CREATE TABLE Funcionarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    Salario DECIMAL(10,2) NOT NULL,
    DataAdmissao DATE NOT NULL
);


Atualize a string de conexão no projeto (arquivo App.config ou no backend):

<connectionStrings>
    <add name="ConexaoMySQL" 
         connectionString="server=localhost;port=3306;database=CadastraFuncionarioDB;uid=seu_usuario;pwd=sua_senha;" 
         providerName="MySql.Data.MySqlClient"/>
</connectionStrings>
Compile e execute o projeto (F5 ou "Start").

Utilize a interface para cadastrar, visualizar e gerenciar funcionários.


⚙ Estrutura do Projeto
Frontend/ → Contém formulários e elementos gráficos da aplicação.
Backend/ → Contém a lógica de negócios e integração com MySQL.
Properties/ → Configurações do projeto, incluindo App.config.

  
🔧 Melhorias Futuras
Gerar relatórios em PDF ou Excel com informações de funcionários.
Melhorar a interface com elementos gráficos mais intuitivos e responsivos.
Backend/ → Contém a lógica de negócios e integração com MySQL.

Properties/ → Configurações do projeto, incluindo App.config.
