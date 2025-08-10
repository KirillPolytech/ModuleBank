FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app

COPY . ./

RUN dotnet tool install --global dotnet-ef --version 9.0.8
ENV PATH="$PATH:/root/.dotnet/tools"

EXPOSE 80

ENTRYPOINT ["sh", "-c", "dotnet ef database update --connection \"$ConnectionStrings__DefaultConnection\" && dotnet run --no-build --urls http://+:80"]
