FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Contactform.Sample.Postgres.csproj", ""]
RUN dotnet restore "Contactform.Sample.Postgres.csproj"
COPY . .
#WORKDIR "/src/Contactform.Sample.Postgres"
RUN dotnet build "Contactform.Sample.Postgres.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contactform.Sample.Postgres.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contactform.Sample.Postgres.dll"]