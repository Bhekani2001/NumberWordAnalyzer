FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NumberWordAnalyzer.API/NumberWordAnalyzer.API.csproj", "NumberWordAnalyzer.API/"]
COPY ["NumberWordAnalyzer.Application/NumberWordAnalyzer.Application.csproj", "NumberWordAnalyzer.Application/"]
COPY ["NumberWordAnalyzer.Domain/NumberWordAnalyzer.Domain.csproj", "NumberWordAnalyzer.Domain/"]
RUN dotnet restore "NumberWordAnalyzer.API/NumberWordAnalyzer.API.csproj"
COPY . .
WORKDIR "/src/NumberWordAnalyzer.API"
RUN dotnet build "NumberWordAnalyzer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NumberWordAnalyzer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NumberWordAnalyzer.API.dll"]
