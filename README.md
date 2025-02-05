This is a small api backend to run locally on my server 

### Dependecies

- [Docker](https://www.docker.com/products/docker-desktop/) and also [docker-compose](https://github.com/docker/compose)

### Building / Running

##### Running api and database toghether

- clone the repo `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git`
- cd into it `cd BackendOlimpiadaIsto`
- *make sure docker desktop is running! and docker-compose command is installed*
- run `docker-compose up -d`. This will start both containers in the background. You can see your running containers in docker desktop. You can stop the container by running `docker-compose down` or by going in the docker desktop app. You can also reset the database by deleting the data under `~/.BackendOlimpiadaIsto/postgres` OR `%USERPROFILE%\BackendOlimpiadaIsto\postgres`

#### Development

I recomend running the database and the api separatley for this. You will require [Dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

- Same as the first option clone the repo and cd into it with `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git` and `cd BackendOlimpiadaIsto`
- make sure the postgressql database is running with `docker-compose up db`.
- cd into the api layer `cd Presentation`
- run the api `dotnet run`