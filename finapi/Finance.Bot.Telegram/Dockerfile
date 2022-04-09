#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Finance.Bot.Telegram/Finance.Bot.Telegram.csproj", "Finance.Bot.Telegram/"]
RUN dotnet restore "Finance.Bot.Telegram/Finance.Bot.Telegram.csproj"
COPY . .
WORKDIR "/src/Finance.Bot.Telegram"
RUN dotnet build "Finance.Bot.Telegram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finance.Bot.Telegram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finance.Bot.Telegram.dll"]