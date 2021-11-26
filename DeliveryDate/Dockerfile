FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Mathem/Mathem.csproj", "Mathem/"]
RUN dotnet restore "Mathem/Mathem.csproj"
COPY . .
WORKDIR "/src/Mathem"
RUN dotnet build "Mathem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mathem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mathem.dll"]
