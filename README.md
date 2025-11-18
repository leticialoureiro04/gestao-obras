# Gestão de Obras

Aplicação web em ASP.NET Core MVC (target .NET 9) para gerir obras, clientes e movimentos associados. O projeto usa Entity Framework Core com MySQL para persistência de dados e disponibiliza CRUDs para os principais recursos.

## Funcionalidades
- **Clientes:** registo e consulta de entidades que contratam as obras.
- **Obras:** criação, edição, ativação e associação a clientes, com informação de localização.
- **Materiais:** catálogo de materiais que podem ser lançados em movimentos.
- **Movimentos:** registo de consumos de materiais por obra.
- **Mão de obra:** controlo de horas/custos de trabalho por obra.
- **Pagamentos:** lançamento de pagamentos efetuados para cada obra.

## Estrutura do projeto
- `GestaoObras/Program.cs`: configuração de serviços, pipeline e localidade padrão pt-PT.
- `GestaoObras/Data/AppDb.cs`: `DbContext` com relacionamentos e `DbSet` para todas as entidades.
- `GestaoObras/Controllers/`: controladores MVC para cada recurso (Obras, Clientes, Materiais, Movimentos, Mão de Obra e Pagamentos).
- `GestaoObras/Views/`: vistas Razor organizadas por recurso.

## Pré-requisitos
- SDK .NET 9.0
- MySQL 8.x acessível com credenciais válidas
- [Ferramentas do Entity Framework Core](https://learn.microsoft.com/ef/core/cli/dotnet) instaladas globalmente (`dotnet tool install --global dotnet-ef`)

## Configuração
1. Atualize a string de ligação em `GestaoObras/appsettings.json` ou `appsettings.Development.json` com o host, base de dados e credenciais do MySQL.
2. Restaure dependências:
   ```bash
   dotnet restore
   ```
3. Aplique as migrações para criar/atualizar o esquema:
   ```bash
   dotnet ef database update --project GestaoObras
   ```

## Execução
1. A partir da raiz do repositório, execute:
   ```bash
   dotnet run --project GestaoObras
   ```
2. A aplicação ficará disponível em `https://localhost:5181`.

## Desenvolvimento
- As migrações existentes estão em `GestaoObras/Migrations/` e podem ser geradas com `dotnet ef migrations add <Nome>`.
- Os modelos de domínio residem em `GestaoObras/Models/` e estão anotados com validações de dados quando aplicável.
- A cultura padrão é `pt-PT` com separador decimal ".", configurada em `Program.cs`.
