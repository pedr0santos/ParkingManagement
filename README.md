# Sistema de Gestão de Clientes, Veículos e Faturamento

Este projeto originalmente em **React** foi convertido para **Angular 20** (standalone components).  

## Alterações e Decisões Técnicas

### Frontend
- Conversão de páginas React para Angular standalone components:
  - **ClientesComponent**, **VeiculosComponent**, **FaturamentoComponent**, **CsvUploadComponent**.
- Uso de **HttpClient** para todas as requisições ao backend.
- Gerenciamento de dados assíncronos com **BehaviorSubject** e pipe `async`.
- Templates com `ngFor`, `ngIf` e `ngModel` para bind de formulários e listas.
- Controle de expansão de detalhes (`placas`) nas tabelas, evitando quebra de layout usando `table-layout: fixed`.
- Upload CSV implementado com `FormData` e log detalhado de erros (linha e motivo).
- Rotas configuradas via `RouterModule` com navegação `routerLink` no navbar.

### Backend
- Faturamento ajustado para considerar histórico de posse de veículos:
  - Faturas proporcionais para clientes que iniciam ou encerram no meio do mês.
- Endpoints principais mantidos, apenas adaptados para consumo pelo Angular.
- Importação de CSV com validação e logs detalhados.

### Banco de Dados
- PostgreSQL com EF Core 8.0.
- Criada tabela `HistoricoPosse` para rastrear vigência dos veículos por cliente.
- Ajuste de migrations para evitar conflitos em tabelas já existentes.

---

### Observações
- Projeto agora totalmente Angular 20 SPA, com navegação reativa.
- Regras de faturamento corrigidas e proporcionais baseadas no histórico de posse.
- Relatórios CSV mais claros e detalhados para erros durante importação.


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
ng serve
```
A aplicação ficará disponível em `http://localhost:4200`.  
A api ficará apontando para `http://localhost:54152`.  

### 4.3 Estrutura de Pastas
```
/src/backend        -> API .NET 8
/src/frontend       -> Angular 20
/scripts/seed.sql   -> Criação e seed do banco
/scripts/exemplo.csv-> CSV de exemplo
```

