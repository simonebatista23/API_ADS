Esta API foi desenvolvida em C# / .NET Web API com o objetivo de oferecer os serviços necessários para um sistema de chamados, incluindo autenticação, gerenciamento de usuários e criação de tickets.

🌐 Funcionalidades Principais

A API fornece recursos para:

Autenticação de usuários via login

Geração de token JWT

Cadastro de usuários

Listagem de usuários

Criação de tickets

Listagem de tickets

Toda a comunicação segue o padrão REST.

📂 Estrutura Geral

A solução segue uma organização simples:

Controllers → Endpoints da API  
Services    → Regras de negócio  
Models      → Estruturas de dados
Repositories → Acesso aos dados  

🔐 Autenticação

A API utiliza JWT.
Para acessar rotas protegidas, o cliente deve enviar:

Authorization: Bearer <token>


O token é obtido no endpoint de login.

🔗 Endpoints
Autenticação

POST /api/auth/login → Retorna token JWT

Usuários

POST /api/users → Cria usuário

GET /api/users → Lista usuários

Tickets

POST /api/tickets → Cria ticket

GET /api/tickets → Lista tickets

▶️ Execução

Configurar o appsettings.json

Executar o projeto:

dotnet run
