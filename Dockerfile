FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

RUN dotnet publish src/JobApplicationAssistant.API/JobApplicationAssistant.API.csproj \
    -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["./JobApplicationAssistant.API"]