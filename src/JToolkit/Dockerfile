FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/JToolkit/JToolkit.csproj", "JToolkit/"]
RUN dotnet restore "JToolkit/JToolkit.csproj"
COPY src/ .
WORKDIR "/src/JToolkit"
RUN dotnet build "JToolkit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JToolkit.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT="development"
ENTRYPOINT ["dotnet", "JToolkit.dll"]
