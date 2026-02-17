# Sistema de Gerenciamento de Despesas Pessoais

## 📋 Visão Geral

Sistema backend para gerenciamento de despesas e receitas pessoais e familiares, desenvolvido com foco em **Arquitetura Hexagonal**, padrão de **Casos de Uso** e **Unit of Work**.

O objetivo principal é permitir que usuários individuais e grupos familiares registrem, categorizem e acompanhem suas movimentações financeiras, com geração de relatórios e análises de fluxo de caixa.

---

## 🎯 Objetivos do Projeto

- ✅ Registrar receitas e despesas pessoais
- ✅ Suportar múltiplos usuários e grupos familiares
- ✅ Categorizar transações financeiras
- ✅ Gerar relatórios de saldo e fluxo de caixa
- ✅ Controlar orçamentos mensais por categoria
- ✅ Implementar transações recorrentes
- ✅ Exercitar conceitos de Arquitetura Hexagonal
- ✅ Utilizar padrão Unit of Work para consistência transacional

---

## 🏗️ Arquitetura

O projeto segue os princípios da **Arquitetura Hexagonal** (Ports & Adapters):

### Camadas

```
┌─────────────────────────────────────────┐
│           API Layer                      │
│    (Controllers, REST Endpoints)        │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│      Application Layer                   │
│  (UseCases, DTOs, Interfaces/Ports)    │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│       Domain Layer (Core)                │
│   (Entities, ValueObjects, Rules)       │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│    Infrastructure Layer                  │
│  (Repositories, DbContext, Services)    │
└─────────────────────────────────────────┘
```

### Princípios

- **Domain**: Entidades de negócio puras, sem dependências externas
- **Application**: Casos de uso, DTOs e interfaces (portas)
- **Infrastructure**: Implementações de persistência e serviços externos
- **API**: Camada de apresentação (REST Controllers)

---

## 📦 Entidades do Domínio

### 1. User (Usuário)

Representa um usuário do sistema que pode realizar transações financeiras.

**Atributos:**
- `Id` (Guid): Identificador único
- `Name` (string): Nome completo
- `Email` (string): Email único
- `CreatedAt` (DateTime): Data de criação
- `UpdatedAt` (DateTime): Data de última atualização
- `FamilyGroupId` (Guid?): Referência ao grupo familiar (opcional)

**Regras:**
- Email deve ser único no sistema
- Email deve ser válido
- Nome é obrigatório (mínimo 3 caracteres)
- Pode pertencer a apenas um grupo familiar
- Usuário é o agregado raiz para transações pessoais

---

### 2. FamilyGroup (Grupo Familiar)

Agrupa múltiplos usuários para compartilhamento e acompanhamento conjunto de despesas.

**Atributos:**
- `Id` (Guid): Identificador único
- `Name` (string): Nome do grupo familiar
- `OwnerId` (Guid): Usuário criador/proprietário
- `CreatedAt` (DateTime): Data de criação
- `UpdatedAt` (DateTime): Data de última atualização
- `Members` (ICollection<User>): Membros do grupo

**Regras:**
- Deve ter ao menos um membro (o criador)
- Nome é obrigatório e único por proprietário
- Apenas o proprietário pode adicionar/remover membros
- Ao excluir grupo, transações compartilhadas são reassociadas aos usuários

---

### 3. Category (Categoria)

Classifica as transações financeiras para organização e análise.

**Atributos:**
- `Id` (Guid): Identificador único
- `Name` (string): Nome da categoria
- `Type` (TransactionType): Tipo (Income/Expense)
- `Icon` (string): Ícone representativo
- `Color` (string): Cor hexadecimal (#RRGGBB)
- `UserId` (Guid?): Usuário proprietário (null = categoria padrão do sistema)
- `CreatedAt` (DateTime): Data de criação

**Regras:**
- Categorias padrão são criadas pelo sistema (não editáveis)
- Usuários podem criar categorias personalizadas
- Nome deve ser único por usuário
- Não é possível excluir categoria com transações vinculadas
- Categorias padrão: Salário, Freelance, Alimentação, Transporte, Saúde, Educação, Lazer, Utilidades, Outros

---

### 4. Transaction (Transação)

Representa uma movimentação financeira individual.

**Atributos:**
- `Id` (Guid): Identificador único
- `UserId` (Guid): Usuário que criou a transação
- `CategoryId` (Guid): Categoria da transação
- `Amount` (Money): Valor (sempre positivo)
- `Type` (TransactionType): Tipo (Income/Expense)
- `Description` (string): Descrição da transação
- `Date` (DateTime): Data da transação
- `IsRecurring` (bool): Indica se é recorrente
- `RecurrencePattern` (RecurrencePattern?): Padrão de recorrência
- `RecurrenceEndDate` (DateTime?): Data de término da recorrência
- `ParentTransactionId` (Guid?): ID da transação pai (se recorrente)
- `CreatedAt` (DateTime): Data de registro
- `UpdatedAt` (DateTime): Data de última atualização
- `IsFamilyShared` (bool): Indica se é compartilhada no grupo familiar

**Regras:**
- Valor deve ser sempre positivo (tipo define entrada/saída)
- Data da transação não pode ser futura
- Descrição é obrigatória (mínimo 3 caracteres)
- Categoria é obrigatória
- Tipo é obrigatório (Income/Expense)
- Transações recorrentes geram automaticamente instâncias futuras
- Valor mínimo: 0.01
- Valor máximo: 999.999.999,99

---

### 5. Budget (Orçamento)

Define limites de gastos por categoria para um período específico.

**Atributos:**
- `Id` (Guid): Identificador único
- `UserId` (Guid): Usuário proprietário
- `CategoryId` (Guid): Categoria vinculada
- `Amount` (Money): Valor limite mensal
- `Month` (int): Mês de referência (1-12)
- `Year` (int): Ano de referência
- `AlertPercentage` (int): Percentual para alerta (padrão 80)
- `CreatedAt` (DateTime): Data de criação
- `UpdatedAt` (DateTime): Data de última atualização

**Regras:**
- Apenas uma entrada por categoria/mês/ano/usuário
- Valor deve ser positivo
- Mês deve estar entre 1 e 12
- Ano deve ser maior ou igual ao ano atual
- Sistema emite alerta ao atingir `AlertPercentage`% do limite
- Cálculo considerando apenas despesas do período

---

### 6. ValueObject: Money (Objeto de Valor)

Representa um valor monetário imutável.

**Atributos:**
- `Amount` (decimal): Valor decimal
- `Currency` (string): Moeda (padrão: "BRL")

**Regras:**
- Imutável após criação
- Valor nunca pode ser negativo
- Suporta operações matemáticas (soma, subtração)
- Igualdade baseada em valor e moeda

---

### 7. Enum: TransactionType

Tipo de transação financeira.

**Valores:**
- `Income`: Receita/Entrada de dinheiro
- `Expense`: Despesa/Saída de dinheiro

---

### 8. Enum: RecurrencePattern

Padrão de recorrência de transações.

**Valores:**
- `Daily`: Diariamente
- `Weekly`: Semanalmente
- `BiWeekly`: A cada duas semanas
- `Monthly`: Mensalmente
- `Quarterly`: Trimestralmente
- `Annually`: Anualmente

---

## 📐 Regras de Negócio

### RN01 - Registro de Transações

```
Quando um usuário registra uma transação:
- Valor DEVE ser sempre positivo (≥ 0.01)
- Tipo (Income/Expense) DEVE ser informado
- Data da transação NÃO PODE ser futura
- Descrição DEVE ser informada (mínimo 3 caracteres)
- Categoria DEVE ser válida e pertencer ao usuário
- Sistema calcula automaticamente o impacto no saldo
```

**Validações:**
- ✓ Valor > 0.01
- ✓ Data <= Hoje
- ✓ Descrição com 3 a 500 caracteres
- ✓ Categoria existe e é acessível ao usuário

---

### RN02 - Transações Recorrentes

```
Transações podem ser marcadas como recorrentes:
- Sistema DEVE gerar automaticamente instâncias futuras
- Usuário PODE editar/cancelar transações individuais da série
- Padrão de recorrência PODE ser: Diário, Semanal, Quinzenal, Mensal, Trimestral, Anual
- Recorrência PODE ter data de término opcional
```

**Comportamento:**
- Transação criada com recorrência gera próximas instâncias
- Editar uma instância individual não afeta outras
- Cancelar recorrência desativa geração de futuras
- Transações recorrentes exibem referência à transação pai

---

### RN03 - Categorias

```
Categorias organizam as transações:
- Sistema FORNECE categorias padrão (não editáveis)
- Usuários PODEM criar categorias personalizadas
- Categorias POSSUEM tipo (Receita/Despesa)
- Categoria NÃO PODE ser excluída se tiver transações vinculadas
```

**Categorias Padrão do Sistema:**
- Receitas: Salário, Freelance, Investimentos, Outros
- Despesas: Alimentação, Transporte, Saúde, Educação, Lazer, Utilidades, Outros

**Validações:**
- ✓ Nome único por usuário
- ✓ Cor deve ser válida (formato hexadecimal)
- ✓ Ícone deve ser suportado pelo sistema

---

### RN04 - Grupos Familiares

```
Grupos facilitam compartilhamento familiar:
- Membros DO grupo PODEM visualizar todas as transações compartilhadas
- Cada transação PERTENCE a um usuário específico
- Transações PODEM ser marcadas como compartilhadas (IsFamilyShared)
- Relatórios PODEM ser individuais ou consolidados do grupo
- Apenas proprietário PODE gerenciar membros
```

**Comportamento:**
- Usuário ao criar grupo torna-se proprietário
- Proprietário pode adicionar/remover membros
- Membro sai do grupo: transações pessoais permanecem, compartilhadas são desvinculadas
- Relatório familiar consolida dados de todos os membros

---

### RN05 - Orçamentos

```
Controle de limite de gastos:
- Usuário DEFINE limite de gastos por categoria/mês
- Sistema EMITE alerta ao atingir 80% (configurável)
- Sistema NOTIFICA ao ultrapassar limite
- Cálculo CONSIDERA apenas despesas do período
```

**Algoritmo de Cálculo:**
```
Total Gasto = Soma de todas as despesas da categoria no mês
Percentual Utilizado = (Total Gasto / Limite) * 100

Se Percentual >= 80% e < 100%: Alerta "Aproximando do limite"
Se Percentual >= 100%: Alerta "Limite ultrapassado"
```

**Validações:**
- ✓ Apenas uma entrada por categoria/mês/ano
- ✓ Valor limite > 0
- ✓ AlertPercentage entre 50 e 99

---

### RN06 - Cálculos e Relatórios

```
Geração de análises financeiras:
- SALDO = Soma de receitas - Soma de despesas (período)
- FLUXO DE CAIXA = Movimentação detalhada por data
- ANÁLISE POR CATEGORIA = Distribuição percentual de gastos
- COMPARATIVO MENSAL = Evolução ao longo dos meses
```

**Fórmulas:**

```
Saldo Geral = ΣReceitas - ΣDespesas

Saldo por Categoria = ΣReceitas(categoria) - ΣDespesas(categoria)

% Gasto por Categoria = (ΣDespesas(categoria) / ΣDespesas(total)) * 100

Evolução Mensal = Saldo(mês N) - Saldo(mês N-1)
```

**Períodos Suportados:**
- Hoje
- Última semana
- Último mês
- Últimos 3 meses
- Últimos 6 meses
- Último ano
- Período customizado

---

### RN07 - Validações de Data

```
Gerenciamento temporal de transações:
- NÃO PERMITIR registro de transações futuras
- PERMITIR edição de transações passadas com auditoria
- EXCLUIR transação apenas se não houver dependências (recorrências)
```

**Regras:**
- Data máxima: Hoje (00:00:00)
- Data mínima: Sem limite (retroativo)
- Edição registra mudanças em log de auditoria
- Exclusão verifica dependências de recorrência

---

### RN08 - Segurança e Privacidade

```
Controle de acesso aos dados:
- Usuário SÓ ACESSA suas próprias transações
- Membros DO grupo ACESSAM transações compartilhadas
- Proprietário DO grupo ACESSA dados consolidados
- Sistema MANTÉM logs de auditoria para operações críticas
```

**Matriz de Acesso:**

| Usuário | Próprias Transações | Compartilhadas (Grupo) | Alheias (Não-grupo) |
|---------|:---:|:---:|:---:|
| Membro comum | ✓ | ✓ | ✗ |
| Proprietário grupo | ✓ | ✓ | ✗ |

**Operações Auditadas:**
- Criar transação
- Editar transação
- Excluir transação
- Criar orçamento
- Editar orçamento
- Adicionar membro ao grupo
- Remover membro do grupo

---

## 🔄 Casos de Uso Principais

### Transações

#### 1. RegisterIncomeUseCase
Registrar uma receita/entrada de dinheiro.

**Entrada:**
```json
{
  "UserId": "guid",
  "CategoryId": "guid",
  "Amount": 1500.00,
  "Description": "Salário mensal",
  "Date": "2026-02-01",
  "IsRecurring": true,
  "RecurrencePattern": "Monthly",
  "RecurrenceEndDate": null,
  "IsFamilyShared": false
}
```

**Saída:**
```json
{
  "TransactionId": "guid",
  "Message": "Receita registrada com sucesso"
}
```

**Fluxo:**
1. Validar entrada (amount > 0, date <= hoje, etc)
2. Validar categoria (existe, pertence ao usuário)
3. Criar entidade Transaction com Type = Income
4. Se recorrente, gerar próximas instâncias
5. Persistir via repositório
6. Fazer commit via UnitOfWork

---

#### 2. RegisterExpenseUseCase
Registrar uma despesa/saída de dinheiro.

**Entrada:**
```json
{
  "UserId": "guid",
  "CategoryId": "guid",
  "Amount": 150.00,
  "Description": "Supermercado",
  "Date": "2026-02-08",
  "IsRecurring": false,
  "IsFamilyShared": true
}
```

**Saída:**
```json
{
  "TransactionId": "guid",
  "BudgetAlert": {
    "Percentage": 85,
    "Status": "Warning",
    "Message": "Você utilizou 85% do orçamento para esta categoria"
  },
  "Message": "Despesa registrada com sucesso"
}
```

**Fluxo:**
1. Validar entrada
2. Validar categoria
3. Criar entidade Transaction com Type = Expense
4. Se recorrente, gerar próximas instâncias
5. Verificar orçamento da categoria para o mês
6. Se ultrapassar, adicionar alerta à resposta
7. Persistir via repositório
8. Fazer commit via UnitOfWork

---

#### 3. UpdateTransactionUseCase
Atualizar os dados de uma transação existente.

**Validações:**
- Transação existe
- Pertence ao usuário
- Data não é futura
- Não é parte de recorrência (individual ou pai?)

---

#### 4. DeleteTransactionUseCase
Excluir uma transação do sistema.

**Validações:**
- Transação existe
- Pertence ao usuário
- Não é pai de transações recorrentes filhas

**Efeito:**
- Se transação pai: cancelar recorrência
- Se transação filho: apenas remove instância
- Registrar auditoria

---

#### 5. GetTransactionsByPeriodUseCase
Listar transações de um usuário em um período.

**Saída:**
```json
{
  "Transactions": [
    {
      "Id": "guid",
      "CategoryName": "Alimentação",
      "Amount": 150.00,
      "Type": "Expense",
      "Description": "Supermercado",
      "Date": "2026-02-08"
    }
  ],
  "TotalIncome": 1500.00,
  "TotalExpense": 450.00,
  "Balance": 1050.00
}
```

---

### Categorias

#### 6. CreateCategoryUseCase
Criar uma categoria personalizada de usuário.

**Validações:**
- Nome único por usuário
- Tipo válido (Income/Expense)
- Cor formato hexadecimal válido

---

#### 7. GetCategoriesUseCase
Listar categorias disponíveis (padrão + personalizadas).

**Filtros:**
- Por tipo (Income/Expense)
- Apenas personalizadas
- Apenas padrão

---

#### 8. UpdateCategoryUseCase
Atualizar dados de categoria personalizadas.

**Nota:** Categorias padrão não podem ser editadas.

---

#### 9. DeleteCategoryUseCase
Excluir categoria personalizada.

**Validações:**
- Não pode excluir categoria padrão
- Não pode excluir se tiver transações vinculadas

---

### Orçamentos

#### 10. CreateBudgetUseCase
Criar um orçamento mensal para uma categoria.

**Validações:**
- Categoria existe e é Expense
- Não existe orçamento para mesma categoria/mês/ano
- Amount > 0
- AlertPercentage entre 50 e 99

---

#### 11. GetBudgetStatusUseCase
Verificar status de um orçamento (quanto foi gasto, quanto falta).

**Saída:**
```json
{
  "BudgetId": "guid",
  "CategoryName": "Alimentação",
  "Limit": 500.00,
  "Spent": 425.00,
  "Remaining": 75.00,
  "PercentageUsed": 85,
  "Status": "Warning"
}
```

---

#### 12. UpdateBudgetUseCase
Alterar o valor limite de um orçamento.

---

### Relatórios

#### 13. GetMonthlyBalanceUseCase
Obter saldo consolidado do mês atual.

**Saída:**
```json
{
  "Month": 2,
  "Year": 2026,
  "TotalIncome": 1500.00,
  "TotalExpense": 450.00,
  "Balance": 1050.00,
  "ByCategory": [
    {
      "CategoryName": "Alimentação",
      "Amount": 300.00,
      "Percentage": 66.67
    },
    {
      "CategoryName": "Transporte",
      "Amount": 150.00,
      "Percentage": 33.33
    }
  ]
}
```

---

#### 14. GetCategoryAnalysisUseCase
Análise detalhada de gastos por categoria.

**Saída:**
```json
{
  "Categories": [
    {
      "CategoryName": "Alimentação",
      "TotalSpent": 300.00,
      "PercentageOfTotal": 66.67,
      "TransactionCount": 3,
      "AverageSpentPerTransaction": 100.00
    }
  ]
}
```

---

#### 15. GetCashFlowUseCase
Fluxo de caixa detalhado no período.

**Saída:**
```json
{
  "Dates": [
    {
      "Date": "2026-02-01",
      "Income": 1500.00,
      "Expense": 0.00,
      "DailyBalance": 1500.00,
      "CumulativeBalance": 1500.00
    },
    {
      "Date": "2026-02-08",
      "Income": 0.00,
      "Expense": 150.00,
      "DailyBalance": -150.00,
      "CumulativeBalance": 1350.00
    }
  ]
}
```

---

#### 16. GetComparativeReportUseCase
Comparação de períodos (mês a mês, etc).

---

### Grupos Familiares

#### 17. CreateFamilyGroupUseCase
Criar um novo grupo familiar.

---

#### 18. AddMemberToGroupUseCase
Adicionar um membro ao grupo familiar.

**Validações:**
- Chamador é proprietário
- Usuário existe
- Usuário não está em outro grupo
- Usuário não está já no grupo

---

#### 19. RemoveMemberFromGroupUseCase
Remover um membro do grupo.

**Efeito:**
- Transações pessoais do membro permanecem
- Transações compartilhadas são desvinculadas

---

#### 20. GetFamilyConsolidatedReportUseCase
Relatório consolidado do grupo familiar.

**Saída:**
```json
{
  "GroupName": "Família Silva",
  "Members": ["João", "Maria"],
  "TotalIncome": 3000.00,
  "TotalExpense": 900.00,
  "Balance": 2100.00,
  "ByMember": [
    {
      "MemberName": "João",
      "Income": 1500.00,
      "Expense": 450.00,
      "Balance": 1050.00
    },
    {
      "MemberName": "Maria",
      "Income": 1500.00,
      "Expense": 450.00,
      "Balance": 1050.00
    }
  ]
}
```

---

## 🛠️ Padrões de Implementação

### Unit of Work Pattern

```csharp
public interface IUnitOfWork : IDisposable
{
    ITransactionRepository Transactions { get; }
    ICategoryRepository Categories { get; }
    IBudgetRepository Budgets { get; }
    IUserRepository Users { get; }
    IFamilyGroupRepository FamilyGroups { get; }
    
    Task<int> CommitAsync();
    Task RollbackAsync();
}
```

**Responsabilidades:**
- Coordenar repositórios
- Gerenciar transações de banco
- Garantir consistência entre agregados
- Fazer commit/rollback atômico

---

### Repository Pattern

Cada agregado possui um repositório isolado:

```csharp
public interface ITransactionRepository
{
    Task Add(Transaction transaction);
    Task Update(Transaction transaction);
    Task Delete(Guid id);
    Task<Transaction?> GetById(Guid id);
    Task<IEnumerable<Transaction>> GetByUserId(Guid userId);
    Task<IEnumerable<Transaction>> GetByPeriod(Guid userId, DateTime start, DateTime end);
}
```

---

### Value Objects

```csharp
public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0) throw new InvalidOperationException("Valor não pode ser negativo");
        Amount = amount;
        Currency = currency;
    }
    
    public bool Equals(Money other) =>
        Amount == other.Amount && Currency == other.Currency;
}
```

---

### Use Case Base

```csharp
public abstract class UseCase<TInput, TOutput>
{
    protected readonly IUnitOfWork _unitOfWork;
    
    protected UseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public abstract Task<TOutput> Execute(TInput input);
}
```

---

## 📊 Modelo de Dados

```
Users
├── Id (PK)
├── Name
├── Email (UNIQUE)
├── FamilyGroupId (FK, NULLABLE)
├── CreatedAt
└── UpdatedAt

FamilyGroups
├── Id (PK)
├── Name
├── OwnerId (FK -> Users)
├── CreatedAt
└── UpdatedAt

Categories
├── Id (PK)
├── Name
├── Type (Income/Expense)
├── Icon
├── Color
├── UserId (FK, NULLABLE)
└── CreatedAt

Transactions
├── Id (PK)
├── UserId (FK -> Users)
├── CategoryId (FK -> Categories)
├── Amount
├── Type (Income/Expense)
├── Description
├── Date
├── IsRecurring
├── RecurrencePattern (NULLABLE)
├── RecurrenceEndDate (NULLABLE)
├── ParentTransactionId (FK, NULLABLE)
├── IsFamilyShared
├── CreatedAt
└── UpdatedAt

Budgets
├── Id (PK)
├── UserId (FK -> Users)
├── CategoryId (FK -> Categories)
├── Amount
├── Month
├── Year
├── AlertPercentage
├── CreatedAt
└── UpdatedAt

AuditLogs
├── Id (PK)
├── UserId (FK -> Users)
├── EntityType
├── EntityId
├── Action (Create/Update/Delete)
├── OldValues (JSON)
├── NewValues (JSON)
└── Timestamp
```

---

## 🔐 Restrições de Segurança

### Autenticação
- Implementar JWT para autenticação
- Tokens com expiração de 24 horas
- Refresh token para renovação

### Autorização
- Cada endpoint valida se usuário pode acessar recurso
- Usuário não logado = acesso negado
- Membro comum não pode editar dados de outros
- Apenas proprietário pode gerenciar grupo

### Auditoria
- Todas operações CRUD registradas
- Histórico de valores antigos/novos
- IP e timestamp registrados
- Retenção de 2 anos

---

## 📚 Referências Tecnológicas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM
- **MySQL 8.0+**: Banco de dados relacional
- **Docker**: Containerização
- **xUnit**: Testes unitários
- **Moq**: Mocks para testes

---

## 📈 Próximas Evoluções

- [ ] Integração com APIs de câmbio para múltiplas moedas
- [ ] Previsões de fluxo de caixa (ML)
- [ ] Notificações em tempo real
- [ ] Anexos e comprovantes (storage)
- [ ] Integração com bancos (open banking)
- [ ] Mobile app nativa
- [ ] Dashboard com gráficos interativos
- [ ] Exportação em PDF/CSV
- [ ] Metas financeiras e rastreamento

---

**Versão:** 1.0  
**Data:** Fevereiro 2026  
**Status:** Detalhamento Completo

