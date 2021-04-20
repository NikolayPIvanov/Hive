Migrations

`dotnet ef migrations add "AddingEntities" --project src\GigManagement\Gig.Infrastructure --startup-project src\LooselyCoupled --output-dir Persistence\Migrations --context GigManagementDbContext`

Update database

`dotnet ef database update --project src\GigManagement\Gig.Infrastructure --startup-project src\LooselyCoupled --context GigManagementDbContext`