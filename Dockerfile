FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
RUN apt-get update && apt-get dist-upgrade -y && apt-get install 
COPY . ./
RUN dotnet restore src/Application/Application.csproj
RUN dotnet restore tests/UnitTests/UnitTests.csproj  
RUN dotnet test tests/UnitTests/UnitTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet sonarscanner begin /k:"Teste" /d:sonar.host.url="http://127.0.0.1:9000" /d:sonar.login="sqp_edfe2d8d5aee79ac983d8ce7b920fb3cfb6b7b56"
RUN dotnet build
RUN dotnet sonarscanner end /d:sonar.login="sqp_edfe2d8d5aee79ac983d8ce7b920fb3cfb6b7b56"
