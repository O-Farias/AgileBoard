# AgileBoard

Uma API RESTful de um quadro Kanban ágil construída com ASP.NET Core 8.0, Entity Framework Core e SignalR para colaboração em tempo real.

## 🚀 Recursos

- Autenticação e autorização usando JWT
- CRUD completo de quadros, listas e cartões 
- Atualização em tempo real usando SignalR
- Validação de dados com FluentValidation
- Logging estruturado com Serilog
- Tratamento global de erros
- API documentada com Swagger/OpenAPI

## 🛠️ Tecnologias

- [ASP.NET Core 8.0](https://docs.microsoft.com/pt-br/aspnet/core/?view=aspnetcore-8.0)
- [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server)
- [SignalR](https://docs.microsoft.com/pt-br/aspnet/core/signalr)
- [JWT Bearer Authentication](https://docs.microsoft.com/pt-br/aspnet/core/security/authentication)
- [FluentValidation](https://fluentvalidation.net/)
- [Serilog](https://serilog.net/)
- [Swagger/OpenAPI](https://swagger.io/)

## 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)

## 🔧 Configuração

1. Clone o repositório:
```bash
git clone https://github.com/O-Farias/AgileBoard.git
cd AgileBoard
```

2. Configure a string de conexão no arquivo `appsettings.json`:
```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=AgileBoard;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }
}
```

3. Execute as migrações do banco de dados:
```bash
dotnet ef database update
```

4. Execute o projeto:
```bash
dotnet run
```

A API estará disponível em `https://localhost:5001` e a documentação Swagger em `https://localhost:5001/swagger`.

## 📚 Documentação da API

### Endpoints

#### Autenticação
- POST `/api/auth/login` - Login do usuário

#### Boards
- GET `/api/boards` - Lista todos os quadros
- GET `/api/boards/{id}` - Obtém um quadro específico
- POST `/api/boards` - Cria um novo quadro
- PUT `/api/boards/{id}` - Atualiza um quadro
- DELETE `/api/boards/{id}` - Remove um quadro

#### Lists
- GET `/api/lists` - Lista todas as listas
- GET `/api/lists/{id}` - Obtém uma lista específica
- GET `/api/lists/board/{boardId}` - Lista todas as listas de um quadro
- POST `/api/lists` - Cria uma nova lista
- PUT `/api/lists/{id}` - Atualiza uma lista
- DELETE `/api/lists/{id}` - Remove uma lista

#### Cards
- GET `/api/cards` - Lista todos os cartões
- GET `/api/cards/{id}` - Obtém um cartão específico
- GET `/api/cards/list/{listId}` - Lista todos os cartões de uma lista
- POST `/api/cards` - Cria um novo cartão
- PUT `/api/cards/{id}` - Atualiza um cartão
- DELETE `/api/cards/{id}` - Remove um cartão

## 📄 Licença

Este projeto está sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para mais detalhes.