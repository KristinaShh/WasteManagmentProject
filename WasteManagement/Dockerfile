FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
# copy csproj and restore as distinct layers
COPY *.sln .
COPY WasteManagement/*.csproj ./WasteManagement/
COPY WasteManagement.Data/*.csproj ./WasteManagement.Data/
COPY WasteManagement.Models/*.csproj ./WasteManagement.Models/
COPY WasteManagement.Services/*.csproj ./WasteManagement.Services/
COPY WasteManagement.Tests/*.csproj ./WasteManagement.Tests/ 
RUN dotnet restore

# copy everything else and build app
COPY WasteManagement/. ./WasteManagement/
COPY WasteManagement.Data/. ./WasteManagement.Data/
COPY WasteManagement.Models/. ./WasteManagement.Models/
COPY WasteManagement.Services/. ./WasteManagement.Services/
COPY WasteManagement.Tests/. ./WasteManagement.Tests/ 

WORKDIR "/src/WasteManagement"
RUN dotnet publish -c Release -o out 

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WasteManagement.dll"]