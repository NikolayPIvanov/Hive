Migrations

`dotnet ef migrations add "AddingEntitiesInInvesting" --project src\Investing\Investing.Infrastructure --startup-project src\LooselyCoupled --output-dir Persistence\Migrations --context InvestingDbContext`

Update database

`dotnet ef database update --project src\Investing\Investing.Infrastructure --startup-project src\LooselyCoupled --context InvestingDbContext`