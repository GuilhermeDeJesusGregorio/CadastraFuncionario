CadastraFuncionario
Sistema desktop desenvolvido em C# para auxiliar o gerenciamento de recursos humanos (RH), focado no cadastro e manutenção de informações de funcionários.

O projeto permite armazenar, visualizar e gerenciar dados de colaboradores de forma prática e eficiente, utilizando uma interface gráfica amigável com persistência de dados em MySQL.

🌟 Funcionalidades
Cadastro Completo: Permite o registro de novos funcionários com as seguintes informações:

Nome

Cargo

Salário

Data de Admissão

Cor/Raça

Forma de Pagamento

Instituição Bancária

Visualização e Gerenciamento: Interface para listar e visualizar todos os registros cadastrados.

Persistência de Dados: Conexão com banco de dados MySQL para armazenamento permanente das informações.

Interface Amigável: Desenvolvido com Windows Forms para uma experiência desktop intuitiva.

💻 Tecnologias Utilizadas
Componente	Tecnologia	Descrição
Linguagem Principal	C#	Base do desenvolvimento da aplicação desktop.
Framework	.NET Framework	Utilizado para criar a aplicação Windows Forms.
Interface Gráfica	Windows Forms	Para a criação da interface do usuário (UI).
Banco de Dados	MySQL	Sistema de gerenciamento de banco de dados para persistência.
Conexão	MySQL Connector/NET	Biblioteca para integração e comunicação entre C# e MySQL.
IDE	Visual Studio	Ambiente de desenvolvimento recomendado.

Exportar para as Planilhas
🛠 Pré-requisitos
Antes de executar o projeto, você precisará ter os seguintes softwares instalados:

Visual Studio (Versão compatível com .NET Framework).

MySQL Server (Para hospedar o banco de dados).

MySQL Connector/NET (Para a conexão entre C# e MySQL).

MySQL Workbench (Opcional, para gerenciar o banco visualmente).

🚀 Como Configurar e Executar
Siga os passos abaixo para colocar o projeto em funcionamento.

1. Clonar o Repositório
Abra seu terminal ou Git Bash e clone o projeto:

Bash

git clone https://github.com/GuilhermeDeJesusGregorio/CadastraFuncionario.git
2. Configurar o Banco de Dados MySQL
Crie o banco de dados e a tabela de funcionários no seu servidor MySQL:

SQL

-- Criação do Banco de Dados
CREATE DATABASE CadastraFuncionarioDB;

-- Uso do Banco de Dados
USE CadastraFuncionarioDB;

-- Criação da Tabela de Funcionários
CREATE TABLE Funcionarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    Salario DECIMAL(10,2) NOT NULL,
    DataAdmissao DATE NOT NULL,
    -- Campos adicionais sugeridos
    CorRaca VARCHAR(50),
    FormaPagamento VARCHAR(50),
    InstituicaoBancaria VARCHAR(100)
);
3. Abrir e Configurar o Projeto
Abra o projeto (CadastraFuncionario.sln) no Visual Studio.

Localize o arquivo de configuração (App.config ou o arquivo no Backend/ que contém a string de conexão).

Atualize a string de conexão com suas credenciais do MySQL (servidor, usuário e senha) para se conectar ao CadastraFuncionarioDB.

4. Compilar e Executar
Certifique-se de que todas as dependências do MySQL Connector/NET foram resolvidas.

Compile o projeto.

Execute a aplicação (pressione F5 ou clique em "Start" no Visual Studio).

A interface do sistema será exibida, permitindo que você cadastre e gerencie funcionários.

📂 Estrutura do Projeto
O projeto é organizado em camadas para separar a interface gráfica da lógica de negócios e do acesso a dados:

CadastraFuncionario/
├── Backend/          # Contém a lógica de negócios e a integração com o MySQL (acesso a dados).
├── Frontend/         # Contém os formulários e elementos gráficos da aplicação (Windows Forms UI).
├── Dashboard/        # (Se aplicável) Área de visualização/resumo dos dados.
├── Properties/       # Configurações do projeto, incluindo o App.config.
├── App.config        # Arquivo de configuração, onde geralmente fica a string de conexão.
├── CadastraFuncionario.csproj
└── CadastraFuncionario.sln
📈 Melhorias Futuras
O projeto pode ser expandido com as seguintes melhorias:

Relatórios: Adicionar funcionalidade para gerar relatórios detalhados em formatos como PDF ou Excel.

UX/UI: Melhorar a interface gráfica com elementos mais intuitivos, modernos e responsivos.

Funcionalidades CRUD: Implementar as funcionalidades de Edição (Update) e Exclusão (Delete) de registros de funcionários.
