FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY FinanceTools/FinanceTools.csproj FinanceTools/
RUN dotnet restore FinanceTools/FinanceTools.csproj --verbosity diag
COPY FinanceTools/ FinanceTools/
RUN dotnet publish FinanceTools/FinanceTools.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinanceTools.dll"]