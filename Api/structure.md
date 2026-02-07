FinanceManager/
├── src/
│   ├── FinanceManager.Domain/
│   │   ├── Entities/
│   │   │   ├── Account.cs
│   │   │   ├── Transaction.cs
│   │   │   ├── Category.cs
│   │   │   ├── Budget.cs
│   │   │   └── User.cs
│   │   ├── ValueObjects/
│   │   │   ├── Money.cs
│   │   │   └── DateRange.cs
│   │   ├── Enums/
│   │   │   ├── TransactionType.cs
│   │   │   ├── AccountType.cs
│   │   │   └── CategoryType.cs
│   │   └── Exceptions/
│   │       ├── DomainException.cs
│   │       ├── InsufficientFundsException.cs
│   │       └── NotFoundException.cs
│   │
│   ├── FinanceManager.Application/****
│   │   ├── Ports/
│   │   │   ├── IAccountRepository.cs
│   │   │   ├── ITransactionRepository.cs
│   │   │   ├── IBudgetRepository.cs
│   │   │   ├── ICategoryRepository.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── INotificationService.cs
│   │   ├── UseCases/
│   │   │   ├── CreateTransaction/
│   │   │   │   ├── CreateTransactionUseCase.cs
│   │   │   │   ├── CreateTransactionRequest.cs
│   │   │   │   └── CreateTransactionResponse.cs
│   │   │   ├── TransferBetweenAccounts/
│   │   │   ├── CheckBudget/
│   │   │   ├── GenerateMonthlyReport/
│   │   │   └── CreateAccount/
│   │   └── DTOs/
│   │
│   ├── FinanceManager.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── FinanceDbContext.cs
│   │   │   ├── UnitOfWork.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── AccountRepository.cs
│   │   │   │   ├── TransactionRepository.cs
│   │   │   │   ├── BudgetRepository.cs
│   │   │   │   └── CategoryRepository.cs
│   │   │   └── Migrations/
│   │   ├── Logging/
│   │   │   └── LoggingConfiguration.cs
│   │   └── ExternalServices/
│   │       ├── EmailNotificationService.cs
│   │       └── SmsNotificationService.cs
│   │
│   └── FinanceManager.Api/
│       ├── Controllers/
│       │   ├── AccountsController.cs
│       │   ├── TransactionsController.cs
│       │   ├── BudgetsController.cs
│       │   └── ReportsController.cs
│       ├── Middleware/
│       │   └── ExceptionHandlingMiddleware.cs
│       ├── Program.cs
│       └── appsettings.json
│
└── tests/
├── FinanceManager.UnitTests/
│   ├── Domain/
│   │   └── AccountTests.cs
│   └── Application/
│       └── CreateTransactionUseCaseTests.cs
└── FinanceManager.IntegrationTests/
└── TransactionsIntegrationTests.cs