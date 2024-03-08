# Bhasha

Bhasha is the Bengali word for "language". The project's aim is a fully-functional solution for English-speakers to learn the Bengali language. Why? Because there's a considerable number of Bengali people in London, such as my fiancé, and their English-speaking partners who have no clue how to speak to their family members in Kolkata. 

There's no reason for this project to limit itself for a specific language, it's just a question of content. Since other language learning apps do not properly support Bengali, here's my try to add one more!

## Project Structure

The VS solution contains multiple folders:
* `Bhasha` - back- and front-end service (IdentityServer, Blazor Server, Orleans)
* `Bhasha.Tests` - unit tests for front- and back-end components

## Debugging

### Prerequisites
* [Docker](https://docs.docker.com/engine/install/)

### PostgreSQL
Make sure you started docker on your local machine. 
Then run the [PostgreSQL docker image](https://hub.docker.com/_/postgres):
```bash
docker run --name postgres -e POSTGRES_PASSWORD=mysecretpassword -d postgres
```

### Bhasha
Update the *appsettings.Development.json* in the Bhasha project and run it from Rider/VS.