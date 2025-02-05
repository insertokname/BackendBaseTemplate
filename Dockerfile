FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN rm -r ./*/bin
RUN rm -r ./*/obj
RUN dotnet restore "BackendOlimpiadaIsto.sln"
RUN dotnet publish "Presentation/Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]