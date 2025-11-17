# Multi-stage build for DreamBig.Faist API
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY src/DreamBig.Faist.Api/DreamBig.Faist.Api.csproj src/DreamBig.Faist.Api/
COPY src/DreamBig.Faist.Application/DreamBig.Faist.Application.csproj src/DreamBig.Faist.Application/
COPY src/DreamBig.Faist.Domain/DreamBig.Faist.Domain.csproj src/DreamBig.Faist.Domain/
COPY src/DreamBig.Faist.Infrastructure/DreamBig.Faist.Infrastructure.csproj src/DreamBig.Faist.Infrastructure/
COPY src/DreamBig.Faist.Persistence/DreamBig.Faist.Persistence.csproj src/DreamBig.Faist.Persistence/
RUN dotnet restore "src/DreamBig.Faist.Api/DreamBig.Faist.Api.csproj"

# Copy the remaining source
COPY . .

# Publish
WORKDIR /src/src/DreamBig.Faist.Api
RUN dotnet publish "DreamBig.Faist.Api.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Configure Kestrel to listen on HTTP 8080 and HTTPS 8443
ENV ASPNETCORE_URLS=http://+:8080;https://+:8443
ENV ASPNETCORE_ENVIRONMENT=Production

# Default DB settings (override in runtime as needed)
ENV DB_HOST=postgres
ENV DB_PORT=5432
ENV DB_USER=postgres
ENV DB_PASSWORD=postgres

EXPOSE 8080 8443

COPY --from=build /app/publish .

# Create and configure a development HTTPS certificate so HTTPS redirection works in containers
# Generate with SDK image and copy here
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS certgen
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p "changeit" -t

FROM final AS withcert
WORKDIR /app
COPY --from=certgen /https/aspnetapp.pfx /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=changeit
ENTRYPOINT ["dotnet", "DreamBig.Faist.Api.dll"]
