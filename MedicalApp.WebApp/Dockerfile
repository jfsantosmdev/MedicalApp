#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MedicalApp.WebApp/MedicalApp.WebApp.csproj", "MedicalApp.WebApp/"]
RUN dotnet restore "MedicalApp.WebApp/MedicalApp.WebApp.csproj"
COPY . .
WORKDIR "/src/MedicalApp.WebApp"
RUN dotnet build "MedicalApp.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedicalApp.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalApp.WebApp.dll"]