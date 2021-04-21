Migrations

`dotnet ef migrations add "AddingEntities" --project src\Billing\Billing.Infrastructure --startup-project src\LooselyCoupled --output-dir Persistence\Migrations --context BillingDbContext`

Update database

`dotnet ef database update --project src\Billing\Billing.Infrastructure --startup-project src\LooselyCoupled --context BillingDbContext`