# ShortnerUrl

API para encurtamento de URLs desenvolvida em .NET com PostgreSQL.

## Tecnologias

- .NET 8
- Entity Framework Core
- PostgreSQL
- Redis
- Npgsql

## Configuração

### Pré-requisitos

- .NET 8 SDK
- Docker e Docker Compose

### Subir os containers
```bash
docker-compose up -d
```

Serviços disponíveis:

| Serviço  | Porta | Descrição           |
|----------|-------|---------------------|
| Postgres | 5432  | Banco de dados      |
| Redis    | 6379  | Cache               |
| PgAdmin  | 5050  | Interface do banco  |

### Acessar PgAdmin

- URL: http://localhost:5050
- Email: admin@admin.com
- Senha: admin

Para conectar ao Postgres no PgAdmin:
- Host: postgres
- Porta: 5432
- Usuário: postgres
- Senha: postgres

### Connection String

Configure no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=sngpcdb;Username=postgres;Password=postgres"
  }
}
```

### Restaurar ferramentas locais
```bash
dotnet tool restore
```

### Rodar migrations
```bash
dotnet ef migrations add InitialCreate --project .\ShortnerUrl.Api\
dotnet ef database update --project .\ShortnerUrl.Api\
```

### Executar a aplicação
```bash
dotnet run --project .\ShortnerUrl.Api\
```

## Estrutura do banco

### Tabelas

- **tb_users** — usuários do sistema
- **tb_roles** — perfis de acesso
- **tb_links** — URLs encurtadas

### Relacionamentos

- User → Role (N:1)
- User → Links (1:N)

## Funcionalidades

- Cadastro de usuários
- Autenticação com refresh token
- Criação de URLs encurtadas
- Controle de acesso por roles
