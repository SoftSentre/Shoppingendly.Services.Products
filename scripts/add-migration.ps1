Param (
    [string]$migrationName = $(Read-Host 'enter a migration name')
)

Invoke-Expression "dotnet ef migrations add $migrationName --project ./src/Infrastructure/SoftSentre.Shoppingendly.Services.Products.Infrastructure.csproj --startup-project ./src/WebApi/SoftSentre.Shoppingendly.Services.Products.WebApi.csproj --output-dir ../WebApi/Migrations"