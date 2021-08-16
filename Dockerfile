FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AuthUtility.csproj", ""]
RUN dotnet restore "./AuthUtility.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AuthUtility.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AuthUtility.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AuthUtility.dll"]