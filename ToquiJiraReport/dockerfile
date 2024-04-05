FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy everything
COPY ./src/01.Services/01.JiraReport ./01.Services/01.JiraReport/
COPY ./src/02.CommonLibs ./02.CommonLibs/

# Restore as distinct layers
WORKDIR /src/01.Services/01.JiraReport/TJR.JiraReport.Api
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c Release -o /src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# SET Environment as Staging
#ENV ASPNETCORE_ENVIRONMENT=Staging

# if you want to specify other internal port use this line. by default is http://+:80 in port 80
#ENV ASPNETCORE_URLS=http://+:40000

WORKDIR /src
COPY --from=build-env /src/out .
ENTRYPOINT ["dotnet", "TJR.JiraReport.Api.dll"]