FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY BackendBaseTemplate.sln /src/

COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Presentation/Presentation.csproj Presentation/

RUN dotnet restore "BackendBaseTemplate.sln"

COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/
COPY Presentation/ Presentation/

RUN rm -r ./*/bin || true
RUN rm -r ./*/obj || true
RUN dotnet restore "BackendBaseTemplate.sln"
RUN dotnet publish "Presentation/Presentation.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]