#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MedicalApp.WebApi/MedicalApp.WebApi.csproj", "MedicalApp.WebApi/"]
COPY ["MedicalApp.DataAccess/MedicalApp.DataAccess.csproj", "MedicalApp.DataAccess/"]
COPY ["MedicalApp.Entities/MedicalApp.Entities.csproj", "MedicalApp.Entities/"]
COPY ["MedicalApp.Abstractions/MedicalApp.Abstractions.csproj", "MedicalApp.Abstractions/"]
COPY ["MedicalApp.Services/MedicalApp.Services.csproj", "MedicalApp.Services/"]
COPY ["MedicalApp.Repository/MedicalApp.Repository.csproj", "MedicalApp.Repository/"]
COPY ["MedicalApp.Application/MedicalApp.Application.csproj", "MedicalApp.Application/"]
RUN dotnet restore "MedicalApp.WebApi/MedicalApp.WebApi.csproj"
COPY . .
WORKDIR "/src/MedicalApp.WebApi"
RUN dotnet build "MedicalApp.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedicalApp.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalApp.WebApi.dll"]