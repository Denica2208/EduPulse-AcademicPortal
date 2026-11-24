FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY AcademicPortal.sln ./
COPY src/AcademicPortal.API/AcademicPortal.API.csproj src/AcademicPortal.API/
RUN dotnet restore src/AcademicPortal.API/AcademicPortal.API.csproj
COPY src/AcademicPortal.API/ src/AcademicPortal.API/
WORKDIR /src/src/AcademicPortal.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AcademicPortal.API.dll"]
