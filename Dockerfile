FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
COPY docker/wait-for-it.sh /wait-for-it.sh
RUN chmod +x wait-for-it.sh
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
ARG Configuration=Debug
WORKDIR /src
COPY ./src/*.sln ./
COPY ./src/Application/*.csproj ./Application/
COPY ./src/ApplicationTest/*.csproj ./ApplicationTest/
COPY ./src/BookCrossingBackEnd/*.csproj ./BookCrossingBackEnd/
COPY ./src/Domain/*.csproj ./Domain/
COPY ./src/Infastructure/*.csproj ./Infastructure/
RUN dotnet restore
COPY ./src/ ./
WORKDIR /src/BookCrossingBackEnd
RUN dotnet build -c $Configuration -o /app

FROM builder AS publish
ARG Configuration=Debug
RUN dotnet publish -c $Configuration -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookCrossingBackEnd.dll"]
