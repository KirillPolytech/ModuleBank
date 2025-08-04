FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY BankAccount.csproj ./
RUN dotnet restore BankAccount.csproj

COPY . ./
RUN dotnet build BankAccount.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BankAccount.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "BankAccount.dll"]