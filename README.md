# Bhasha

Bhasha is the Bengali word for "language". The project's aim is a fully-functional solution for English-speakers to learn the Bengali language. Why? Because there's a considerable number of Bengali people in London, such as my fiancé, and their English-speaking partners who have no clue how to speak to their family members in Kolkata. 

There's no reason for this project to limit itself for a specific language, it's just a question of content. Since other language learning apps do not properly support Bengali, here's my try to add one more!

## Project Structure

The VS solution contains multiple folders:
* `Bhasha` - back- and front-end service (IdentityServer, Blazor Server, Orleans)
* `Bhasha.Tests` - unit tests for front- and back-end components

## Build & Deployment

### Prerequisites
* [Docker](https://docs.docker.com/engine/install/)
* Kubernetes (incl. `kubectl`, can be [enabled in docker](https://docs.docker.com/desktop/kubernetes/))

### Docker
Make sure you started docker on your local machine. 
Then create a local docker image for bhasha:
```bash
cd /path/to/repository
docker build -f Bhasha/Dockerfile --force-rm -t bhasha . --no-cache
```

### Kubernetes

#### MAC OS
Deploy MongoDB, which is required by Bhasha, to your local k8s cluster:
```bash
kubectl apply -f dev/mongo
```

Now, deploy Bhasha to your local k8s cluster:
```bash
kubectl apply -f dev/bhasha
```

Now you should be able to access Bhasha via:
http://localhost
