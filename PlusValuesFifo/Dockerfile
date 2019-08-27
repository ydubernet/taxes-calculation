FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY --from=PlusValuesFifo/ClientApp . .

COPY ["PlusValuesFifo/PlusValuesFifo.csproj", "PlusValuesFifo/"]
RUN dotnet restore "PlusValuesFifo/PlusValuesFifo.csproj"
COPY . .
WORKDIR "/src/PlusValuesFifo"
# https://stackoverflow.com/questions/51918919/how-to-integrate-npm-install-into-asp-net-core-2-1-docker-build
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_6.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install
RUN dotnet build "PlusValuesFifo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PlusValuesFifo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PlusValuesFifo.dll"]
