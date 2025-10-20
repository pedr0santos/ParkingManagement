### Stack de Referência
- **Backend**: .NET 8 Web API + EF Core + PostgreSQL  
- **Frontend**: React (Vite) + React Router + React Query  
- **Sem containers**: a conexão é configurada diretamente em `appsettings.json`.  

> É permitido substituir React por Angular/Vue e/ou trocar o ORM, desde que o escopo seja mantido e as decisões sejam explicadas no README. O boilerplate fornecido está em React com JavaScript.  

### Execução Local

#### Banco PostgreSQL
1. Crie um banco local (ex.: `parking_test`) e ajuste a `ConnectionString` em `appsettings.json`, se necessário.  
2. Rode o seed pelo terminal (bash/WSL):  
   ```bash
   psql -h localhost -U postgres -d parking_test -f scripts/seed.sql
   ```  
   Caso utilize Windows sem WSL, execute o script pelo gerenciador de banco de dados de sua preferência (ex.: DBeaver).  

#### Backend
```bash
cd src/backend
dotnet restore
dotnet run
```
A API será iniciada (por padrão) em `http://localhost:5000`. Swagger ativado em `/swagger`.  

#### Frontend
```bash
cd src/frontend
npm install
npm run dev
```
A aplicação ficará disponível em `http://localhost:5173`.  
Configure `VITE_API_URL` caso seja necessário apontar para outra porta.  

### 4.3 Estrutura de Pastas
```
/src/backend        -> API .NET 8
/src/frontend       -> React (Vite)
/scripts/seed.sql   -> Criação e seed do banco
/scripts/exemplo.csv-> CSV de exemplo
```

