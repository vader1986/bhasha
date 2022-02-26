# Bhasha

Bhasha is the Bengali word for "language". The project's aim is a fully-functional solution for English-speakers to learn the Bengali language. Why? Because there's a considerable number of Bengali people in London, such as my fiancé, and their English-speaking partners who have no clue how to speak to their family members in Kolkata. 

There's no reason for this project to limit itself for a specific language, it's just a question of content. Since other language learning apps do not properly support Bengali, here's my try to add one more!

## Project Structure

The VS solution contains multiple folders:
* `Bhasha.Web` - back- and front-end service based on Blazor Server
* `Bhasha.Web.Tests` - unit tests for front- and back-end components

## Build & Deployment

### Prerequisites
* [Docker](https://docs.docker.com/engine/install/)
* Kubernetes (incl. `kubectl`, can be [enabled in docker](https://docs.docker.com/desktop/kubernetes/))

### Build
```bash
docker-compose build --no-cache
```

### Deployment for Development

#### MAC OS
Assuming you've got docker installed on your machine with kubernetes enabled, you can deploy required infrastructure for a local development environment:
```bash
kubectl apply -f deploy/infrastructure -R
```

First, we need to create an HTTPS certificate for the identity server and both Web APIs. Create a [self-signing HTTPS certificate](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-5.0):
```bash
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
dotnet dev-certs https --trust
```

In case you already got a developer certificate, you have to remove and re-create it:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
dotnet dev-certs https --trust
```

Deploy the HTTPS certificate as a secret to kubernetes:
```bash
kubectl create secret generic cert-secret --from-file=cert.pfx=${HOME}/.aspnet/https/aspnetapp.pfx
```

Now you can deploy all services to your local kubernetes cluster:
```bash
kubectl apply -f deploy/development -R
```

### Deployment for Production

TODO
