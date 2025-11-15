Backend api template

### Design Patterns Employed

- **CQRS (Command Query Responsibility Segregation)**
- **CLEAN architecture**
- **MVC**
- **Dependency Injection**
- **Repository Pattern**

### Features provided

- Easy to run since it is contained in a `dockerfile`

- Some basic generic commands for future database entities and features

- Admin users that can easily manage database through api endpoints

- Secure login through JWT and OAuth

- Rate limitng for login and most other public endpoints

- Easily extensible secrets manager class that warns about public environment variables

# Building / Running

### Dependencies

- Google OAuth and Vertex ai. Setup detailed below.
- [Docker](https://www.docker.com/products/docker-desktop/) and also [docker-compose](https://github.com/docker/compose)

##### Setting up required gcp services

- Make a gcp project [here](https://console.cloud.google.com)
- Setup OAUTH [here](https://console.cloud.google.com/auth/clients). The app is registered as a web app. It has 2 authorized redirects `http://localhost:8888/signin-google` and `https://{YOUR.DOMAIN}/signin-google`. After setting this up copy you oauth secret and oauth id.
- Setup vertex ai [here](https://console.cloud.google.com/vertex-ai/studio/multimodal) (enable required apis)
- make a google service key from [here](https://console.cloud.google.com/iam-admin/serviceaccounts) click on your project, add a service account. Give it any name you wish. Give it the Owner permissions (if you want to be safer you can just give it vertex ai specific permissions). I didn't personally fill out any "Principals with access". After this click on done. Click on the newly created service account. Go to the keys section. Click on add key, create new key. Select json and create. Copy the newly created file into the `Presentation/Environment` path and rename it to `key.json`. 
- You are done! All external services related to gcp are setup.

##### Running api and database together

- clone the repo `git clone https://github.com/insertokname/BackendBaseTemplate.git`
- cd into it `cd BackendBaseTemplate`
- make a file named `.env` in the directory containing the following (replace the placeholder values `{}`):
```
PORT="8888"
DOMAIN_NAME="{YOUR.DOMAIN.NAME}"
GOOGLE_PROJECT_ID="{YOUR_GOOGLE_PROJECT_ID}"
GOOGLE_OAUTH_ID="{YOUR_GOOGLE_OAUTH_ID}"
GOOGLE_OAUTH_SECRET="{YOUR_GOOGLE_OAUTH_SECRET}"
```

- *make sure docker desktop is running! and docker-compose command is installed*
- run `docker-compose --profile http up -d`. This will start the api in http mode and will also start the database service. You can see your running containers in docker desktop or by running `docker-compose ps`. You can stop the container by running `docker-compose --profile http down` or by going in the docker desktop app and stopping them manually. You can also reset the database by deleting the data under `~/.BackendBaseTemplate/postgres` OR `%USERPROFILE%\BackendBaseTemplate\postgres`
- If you are planning on using this for production:
    - **change the username and the password** in the `.env` file
    - don't forget to actually set values for the `_DEFAULT` variables you get warnings about when starting the app. To set these up you have to make an environment variable that doesn't have the `_DEFAULT` suffix, you could set them in the `docker-compose.yaml` file.
    - **Make sure you enable https!** To do so, check the [Enabling https](#enabling-https) section.

#### Development

I recomend running the database and the api separatley for this. You will require docker and docker-compose just like the running part and also [Dotnet 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

- Same as the first option clone the repo and cd into it with `git clone https://github.com/insertokname/BackendBaseTemplate.git` and `cd BackendBaseTemplate`
- make sure the postgressql database is running with `docker-compose up db -d` (you can also stop the db with `docker-compose down db`)
- cd into the api layer `cd Presentation` 
- run the api `dotnet run`


#### Enabling https

- **Https requires you own a domain name!**
- **Make sure port 80 is opened on your server / you have port forwarding for port 80 enabled on your router** This is necessary for the certbot verification!
- Make sure all **http** services are down (`docker-compose --profile http down` will stop them).
- Register your certificate using the `certbot` service provided in the `docker-compose` by running this one of command (**MAKE SURE TO REPLACE `<DOMAIN_NAME>` and `<EMAIL>` with you correct info**):
```
docker-compose run --remove-orphans --service-ports --entrypoint certbot certbot certonly --standalone -d <DOMAIN_NAME> --agree-tos --non-interactive -m <EMAIL>
```
- After this certbot will register your domain with Let's encrypt and you will have valid https!
- You can now run the `docker-compose` command as you normally did but **instead of using `--profile http` use `--profile https`**: `docker-compose --profile https up -d` & `docker-compose --profile https down`

#### Steps to add a new entity:

- Create the new entity derived class inside the Domain/Entities folder
- Make it's create command, you can either do this by creating your own custom create command or by using the generic commands like create handler as shown for the EntityTemplate class
- If you decide to make a custom handler you need to add it to dependency injection in `Presentation\Configuration\ApplicationServicesConfiguration.cs` like so: `builder.Services.AddScoped(typeof(<YOUR_NEW_HANDLER>));`\
- Last thing is add it in the AppDbContext.cs inside the infrastructre/data folder as a DbSet
- migrate changes `dotnet ef migrations add <MIGRATION_NAME>`