FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
RUN apt-get install nodejs

COPY ["PlusValuesFifo/PlusValuesFifo.csproj", "PlusValuesFifo/"]
RUN dotnet restore "PlusValuesFifo/PlusValuesFifo.csproj"
COPY . .
WORKDIR "/src/PlusValuesFifo"
RUN dotnet build "PlusValuesFifo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PlusValuesFifo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PlusValuesFifo.dll"]
