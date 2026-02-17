dotnet ef migrations add FUllNameFieldUser -o Infrastructure/Migrations -p Infrastructure/Infrastructure.csproj -s Api/Api.csproj --verbose


dotnet ef database update -p Infrastructure/Infrastructure.csproj  -s Api/Api.csproj