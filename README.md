# Bhasha

Bhasha is the Bengali word for "language". The project's aim is a fully-functional solution for English-speakers to learn the Bengali language. Why? Because there's a considerable number of Bengali people in London, such as my fiancé, and their English-speaking partners who have no clue how to speak to their family members in Kolkata. 

There's no reason for this project to limit itself for a specific language, it's just a question of content. Since other language learning apps do not properly support Bengali, here's my try to add one more!

## Project Structure

The VS solution contains multiple folders:
* `Bhasha.Common` - general collection of model classes used across the entire project
* `Bhasha.Common.MongoDb` - MongoDB layer to access, add, update and delete user and language data
* `Bhasha.Web` - .NET backend hosting the client app and web api to access data

There's also a _react-app_ named `Bhasha.Web.Client`. 

## Build & Deployment

### Prerequisites
* [NPM](https://www.npmjs.com/get-npm)
* [Docker](https://docs.docker.com/engine/install/)
* Kubernetes (incl. `kubectl`, can be [enabled in docker](https://docs.docker.com/desktop/kubernetes/))

### Build
```bash
cd Bhasha.Web.Client
npm install
npm run build
cd ..
docker-compose build
```

### Deployment
```bash
kubectl apply -f deploy
```
