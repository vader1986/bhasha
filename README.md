# Bhasha

Bhasha is the Bengali word for "language". The project's aim is a fully-functional solution for English-speakers to learn the Bengali language. Why? Because there's a considerable number of Bengali people in London, such as my fiancé, and their English-speaking partners who have no clue how to speak to their family members in Kolkata. 

There's no reason for this project to limit itself for a specific language, it's just a question of content. Since other language learning apps do not properly support Bengali, here's my try to add one more!

## Project Structure

The VS solution contains multiple folders:
* `Bhasha.Common` - general collection of model classes used across the entire project
* `Bhasha.Common.MongoDb` - MongoDB layer to access, add, update and delete user and language data
* `Bhasha.Student.Api` - Web API for language students to access language data (including user stats)
* `Bhasha.Student.Web` - [Blazor UI](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) for language students
* `Bhasha.Author.Api` - Web API for content authors to add translations, chapters, etc.
* `Bhasha.Author.Web` - [Blazor UI](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) for content authors
* `Bhasha.Identity` - [Identity Server](https://github.com/souzartn/IdentityServer4.Samples.Mongo) for user management and authentication

There's also a _react-app_ named `Bhasha.Web.Client`. 

## Build & Deployment

### Prerequisites
* [Docker](https://docs.docker.com/engine/install/)
* Kubernetes (incl. `kubectl`, can be [enabled in docker](https://docs.docker.com/desktop/kubernetes/))

### Build
```bash
docker-compose build --no-cache
```

### Deployment

Assuming you've got docker installed on your machine with kubernetes enabled, you can deploy all required infrastructure for a local development environment:
```bash
kubectl apply -f deploy/infrastructure -R
```

You can also deploy all services in k8s:
```bash
kubectl apply -f deploy/development -R
```
Alternatively, you can just run the services from Visual Studio.

Following URLs are exposed:
* http://localhost:5000/swagger (Bhasha Author API)
* http://localhost:5001/index.html (Bhasha Author Website)
* http://localhost:5002/swagger (Bhasha Student API)
* http://localhost:5003/index.html (Bhasha Student Website)
