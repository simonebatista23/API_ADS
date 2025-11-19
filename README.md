# API — Sistema de Chamados Internos (C#)

Esta é a **API REST desenvolvida em ASP.NET Core**, responsável por gerenciar toda a estrutura do sistema de chamados internos. Ela fornece os endpoints utilizados pelo front-end para autenticação, criação de chamados, listagem, respostas e administração de usuários.

---

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core Web API**
- **C#**
- **Entity Framework Core**
- **JWT Authentication**
- **Injeção de Dependência (DI)**

---

## ⚙️ Funcionalidades da API

| Recurso | Descrição |
|--------|-----------|
| **Autenticação (JWT)** | Login retorna um token JWT utilizado em todas as requisições protegidas. |
| **Gerenciamento de Usuários** | Cadastro, listagem e controle de permissão (Usuário / Admin). |
| **Criação de Chamados** | Endpoint para registrar um novo chamado com título, descrição e categoria. |
| **Listagem de Chamados** | Retorna chamados do usuário logado. |
| **Resposta a Chamados** | Permite inserir mensagens dentro de um chamado existente. |

---

## 📡 Estrutura de Endpoints (Resumo)

> **Abaixo estão listados apenas alguns dos principais endpoints da API.  
A API completa possui mais rotas além dessas.**
### 🔑 Autenticação
- `POST /auth/login` — Realiza login e gera token JWT.

### 👤 Usuários
- `POST /users` — Cria usuário (Admin).  
- `GET /users` — Lista usuários.

### 🎫 Chamados
- `POST /tickets` — Cria um novo chamado.  
- `GET /tickets` — Lista chamados.  
- `GET /tickets/{id}` — Detalhes do chamado.  

---

## 🧱 Estrutura Geral do Projeto

```
/Controllers
/Models
/DTOs
/Services
/Repositories
/Configurations   (JWT, Swagger, CORS)
```

---

## ▶️ Como Executar

1. Instale o .NET SDK  
2. Restaure dependências:  
   `dotnet restore`
3. Rode o projeto:  
   `dotnet run`
4. Acesse a documentação Swagger:  
   `http://localhost:5000/swagger`

---

## ✔️ Conclusão

A API foi desenvolvida de forma simples e organizada, aplicando boas práticas como POO, camadas separadas (Controller / Service / Repository), autenticação JWT e uso de DTOs para padronizar a comunicação com o front-end.
