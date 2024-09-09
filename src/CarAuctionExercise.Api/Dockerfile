FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/CarAuctionExercise.Api/CarAuctionExercise.Api.csproj", "src/CarAuctionExercise.Api/"]
COPY ["src/CarAuctionExercise.Infrastructure/CarAuctionExercise.Infrastructure.csproj", "src/CarAuctionExercise.Infrastructure/"]
COPY ["src/CarAuctionExercise.Application/CarAuctionExercise.Application.csproj", "src/CarAuctionExercise.Application/"]
COPY ["src/CarAuctionExercise.Domain/CarAuctionExercise.Domain.csproj", "src/CarAuctionExercise.Domain/"]

RUN dotnet restore "src/CarAuctionExercise.Api/CarAuctionExercise.Api.csproj"

COPY . .

WORKDIR "/src/src/CarAuctionExercise.Api"

RUN dotnet build "CarAuctionExercise.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarAuctionExercise.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV ASPNETCORE_URLS=http://0.0.0.0:8090;
ENV DOTNET_ENVIRONMENT=Development

WORKDIR /app

COPY --from=publish /app/publish .
COPY src/CarAuctionExercise.Api/entrypoint.sh .
RUN chmod u+x entrypoint.sh

CMD ./entrypoint.sh
