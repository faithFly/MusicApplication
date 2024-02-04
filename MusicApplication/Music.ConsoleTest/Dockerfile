FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Music.ConsoleTest/Music.ConsoleTest.csproj", "Music.ConsoleTest/"]
RUN dotnet restore "Music.ConsoleTest/Music.ConsoleTest.csproj"
COPY . .
WORKDIR "/src/Music.ConsoleTest"
RUN dotnet build "Music.ConsoleTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Music.ConsoleTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Music.ConsoleTest.dll"]
