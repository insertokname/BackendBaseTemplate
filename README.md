This is a small api backend to run locally on my server 

### Design Patterns Employed

- **Repository Pattern**  
    Abstracts data access through a common interface. This is evident in the generic repository implementation that encapsulates CRUD operations.

- **CQRS (Command Query Responsibility Segregation)**  
    Separates read and write operations. Commands (e.g., Create, Delete) and queries (e.g., GetAll, Verify) are handled by distinct handler classes.

- **Dependency Injection**  
    Handlers and repositories are injected into controllers and other services, promoting loose coupling.

- **Domain-Driven Design (DDD)**  
    The application follows DDD principles with a clear separation of concerns through distinct layers (Domain, Application, Infrastructure, Presentation). Domain entities and value objects encapsulate business rules and logic.


### Building / Running

### Dependecies

- [Docker](https://www.docker.com/products/docker-desktop/) and also [docker-compose](https://github.com/docker/compose)

##### Running api and database toghether

- clone the repo `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git`
- cd into it `cd BackendOlimpiadaIsto`
- *make sure docker desktop is running! and docker-compose command is installed*
- run `docker-compose up -d`. This will start both containers in the background. You can see your running containers in docker desktop. You can stop the container by running `docker-compose down` or by going in the docker desktop app. You can also reset the database by deleting the data under `~/.BackendOlimpiadaIsto/postgres` OR `%USERPROFILE%\BackendOlimpiadaIsto\postgres`
- If you are planning on using this for production don't forget to actually set values for the `_DEFAULT` variables in appsettings.json. To set these up you have to make an environment variable that doesn't have the `_DEFAULT` suffix, you could set them in the `docker-compose.yaml` file.

#### Development

I recomend running the database and the api separatley for this. You will require docker and docker-compose just like the running part and also [Dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

- Same as the first option clone the repo and cd into it with `git clone https://github.com/insertokname/BackendOlimpiadaIsto.git` and `cd BackendOlimpiadaIsto`
- make sure the postgressql database is running with `docker-compose up db`.
- cd into the api layer `cd Presentation`
- run the api `dotnet run`