FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ContactForm.Sample.csproj", ""]
RUN dotnet restore "ContactForm.Sample.csproj"
COPY . .
#WORKDIR "/src/ContactForm.Sample"
RUN dotnet build "ContactForm.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactForm.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ContactFormSampleSettings__RecaptchaEnabled=false
ENTRYPOINT ["dotnet", "ContactForm.Sample.dll"]