Migrations

`dotnet ef migrations add "AddingEntitiesInProfiles" --project src\UserProfileManagement\UserProfile.Infrastructure --startup-project src\LooselyCoupled --output-dir Persistence\Migrations --context UserProfileDbContext`

Update database

`dotnet ef database update --project src\UserProfileManagement\UserProfile.Infrastructure --startup-project src\LooselyCoupled --context UserProfileDbContext`