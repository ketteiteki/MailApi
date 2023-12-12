FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY Mail.Application/*.csproj Mail.Application/
COPY Mail.Domain/*.csproj Mail.Domain/
COPY Mail.Infrastructure/*.csproj Mail.Infrastructure/
COPY Mail.IntegrationTests/*.csproj Mail.IntegrationTests/
COPY Mail.WebApi/*.csproj Mail.WebApi/
WORKDIR Mail.WebApi
RUN dotnet restore
COPY . .
RUN dotnet publish Mail.WebApi/Mail.WebApi.csproj -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_ENVIRONMENT Docker
EXPOSE 80
ENTRYPOINT ["dotnet", "Mail.WebApi.dll"]