# CadastraFuncionario

Projeto para cadastro e gerenciamento de funcionários, desenvolvido em C# (.NET Framework) com interface gráfica (WinForms), integrado a banco de dados MySQL.  
Permite realizar operações básicas de CRUD (Criar, Ler, Atualizar e Deletar) sobre os registros de colaboradores.

## Funcionalidades principais

- Cadastro de novos funcionários, com campos como nome, data de admissão, salário, etc.  
- Listagem de funcionários existentes.  
- Edição e exclusão de registros.  
- Integração com banco de dados MySQL para persistência de dados.  
- Interface simples e intuitiva para usuário final (desktop).  

## Tecnologias utilizadas

- Linguagem: C#  
- Plataforma: .NET Framework (versão conforme especificado no projeto)  
- Banco de dados: MySQL  
- Acesso a dados: MySql.Data (Connector/NET)  
- Interface gráfica: Windows Forms (WinForms)  
- Controles padrão do Windows Forms para formulários, tabelas, botões etc.  
- Gerenciamento de pacotes: NuGet  

## Pré-requisitos

- Windows OS compatível com aplicações .NET Framework  
- Instalado o MySQL (versão compatível) ou acesso remoto a um servidor MySQL  
- String de conexão devidamente configurada no arquivo `App.config`  
- Permissões adequadas para criar/alterar tabela no banco de dados  

## Instalação e configuração

1. Clone o repositório:
2.  git clone https://github.com/GuilhermeDeJesusGregorio/CadastraFuncionario.git

Abra o projeto no Visual Studio (versão compatível).

No Gerenciador de Pacotes NuGet, instale ou verifique as dependências:

MySql.Data

Outras bibliotecas conforme listado no projeto (se houver).

Configure a string de conexão no App.config (exemplo):

<connectionStrings>
  <add name="CadastraFuncionarioDB" connectionString="server=localhost;user id=root;password=senha;database=CadastraFuncionarioDB;Persist Security Info=True" providerName="MySql.Data.MySqlClient"/>
</connectionStrings>


Prepare o banco de dados MySQL:

Crie banco de dados: CadastraFuncionarioDB (ou o nome que preferir).

Execute o script SQL de criação da tabela (caso exista no repositório) para criar a tabela Funcionarios com os campos necessários.

Compile e execute o aplicativo.

Ao abrir, utilize a interface para cadastrar, editar, excluir e visualizar funcionários.

Estrutura do projeto

Form1.cs — Tela principal com listagem de funcionários e botões de ação (Adicionar, Editar, Excluir).

Funcionario.cs — Classe de modelo representando um funcionário.

DaoFuncionario.cs — Classe de acesso a dados (DAO) que interage com o banco MySQL.

App.config — Arquivo de configuração com string de conexão e outras definições.

Pastas adicionais, como Recursos, Interfaces, etc, conforme organização do repositório.

Uso

Adicionar funcionário: clique no botão “Adicionar”, preencha os dados e confirme.

Editar funcionário: selecione um registro na listagem e clique em “Editar”.

Excluir funcionário: selecione um registro e clique em “Excluir”.

Visualizar listagem: todos os funcionários cadastrados são exibidos em tabela, com colunas relevantes.

Boas práticas e observações

Trate exceções de conexão com banco de dados (ex: servidor inacessível, credenciais inválidas).

Valide campos de entrada — por exemplo: data de admissão não pode ser futura, salário deve ser número positivo etc.

Considere utilizar uma camada de serviços (Service) se o projeto crescer, para separar lógica de negócios da interface.

Considere migração para Entity Framework ou outro ORM para facilitar manutenção futura.

Faça backup regular do banco de dados para evitar perda de dados.

Contribuição

Contribuições são bem-vindas! Você pode:

Abrir issues relatando bugs ou sugerindo melhorias.

Enviar pull requests com correções, novas funcionalidades ou refatorações.

Sugerir melhorias na interface, adicionar internacionalização, relatórios, exportação/imporação de dados etc.

Licença

Este projeto está licenciado sob a [inserir licença, por exemplo MIT] – veja o arquivo LICENSE para mais detalhes.
   ```bash
   git clone https://github.com/GuilhermeDeJesusGregorio/CadastraFuncionario.git
