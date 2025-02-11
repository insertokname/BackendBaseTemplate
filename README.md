This is a small api backend to run locally on my server 

### Design Patterns Employed

- **Repository Pattern**

- **CQRS (Command Query Responsibility Segregation)**

- **Dependency Injection**

- **DDD (Domain-Driven Design)**

### Features provided

- Easy to run since it is contained in a `dockerfile`

- Some basic generic commands for future database entities and features

- Admin users that can easily manage database through api endpoints

- Secure login through JWT

- Rate limitng for login and most other public endpoints

- Easily extensible secrets manager class that warns about public environment variables

# Building / Running

### Dependecies

- [Docker](https://www.docker.com/products/docker-desktop/) and also [docker-compose](https://github.com/docker/compose)

##### Running api and database toghether

- clone the repo `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git`
- cd into it `cd BackendOlimpiadaIsto`
- *make sure docker desktop is running! and docker-compose command is installed*
- run `docker-compose --profile http up -d`. This will start the api in http mode and will also start the database service. You can see your running containers in docker desktop or by running `docker-compose ps`. You can stop the container by running `docker-compose --profile http down` or by going in the docker desktop app and stopping them manually. You can also reset the database by deleting the data under `~/.BackendOlimpiadaIsto/postgres` OR `%USERPROFILE%\BackendOlimpiadaIsto\postgres`
- If you are planning on using this for production don't forget to actually set values for the `_DEFAULT` variables you get warnings about when starting the app. To set these up you have to make an environment variable that doesn't have the `_DEFAULT` suffix, you could set them in the `docker-compose.yaml` file.

#### Development

I recomend running the database and the api separatley for this. You will require docker and docker-compose just like the running part and also [Dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

- Same as the first option clone the repo and cd into it with `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git` and `cd BackendOlimpiadaIsto`
- make sure the postgressql database is running with `docker-compose up db -d` (you can also stop the db with `docker-compose down db`)
- cd into the api layer `cd Presentation` 
- run the api `dotnet run`