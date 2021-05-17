FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY ./CloudDataProtection.Core/ ./CloudDataProtection.Core/

ENV ProjectName=CloudDataProtection.Gateway

COPY ./${ProjectName}/ ./${ProjectName}

RUN dotnet restore ./${ProjectName}/${ProjectName}.csproj

RUN dotnet publish ./${ProjectName}/${ProjectName}.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/runtime:3.1 as run-env
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "CloudDataProtection.Gateway.dll"]