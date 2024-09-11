@ECHO OFF 
:: This batch file runs sonarqube
TITLE SonarQube
ECHO Please wait... Runing sonarqube Commands.
ECHO ==========================
ECHO Sonar START
ECHO ============================
dotnet sonarscanner begin /k:"Investiment" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="sqp_f709457b560dc01b462fcef39e00df7d4bebdb50" /d:sonar.cs.opencover.reportsPaths=C:/repositorio/InvestimentPlatform/tests/UnitTests/coverage.opencover.xml
ECHO ============================
ECHO BUILD
ECHO ============================
dotnet build
ECHO ============================
ECHO TEST
ECHO ============================
dotnet test C:/repositorio/InvestimentPlatform/tests/UnitTests/UnitTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover 
ECHO ============================
ECHO END
ECHO ============================
dotnet sonarscanner end /d:sonar.login="sqp_f709457b560dc01b462fcef39e00df7d4bebdb50"
PAUSE