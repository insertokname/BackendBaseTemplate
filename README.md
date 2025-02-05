This is a small api backend to run locally on my server 

### Dependecies

- [Dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) + [docker-compose](https://github.com/docker/compose) (for the postgressql database)

### Building / Running

- clone the repo `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git`
- cd into it `cd BackendOlimpiadaIsto`
- *make sure docker desktop is running! and docker-compose command is installed*
- start the database with `docker-compose up -d`. This will start the container and will detach it so it runs in the background. You can see your running containers in docker desktop. You can stop the container by running `docker-compose down` or by going in the docker desktop app. You can also reset the database by deleting data under `~/.BackendOlimpiadaIsto/postgres` OR `%USERPROFILE%\BackendOlimpiadaIsto\postgres`
- cd into the api layer `cd Presentation` 
- run it with dotnet `dotnet run`