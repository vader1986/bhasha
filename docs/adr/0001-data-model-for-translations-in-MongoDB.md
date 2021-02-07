# Data model for translations in MongoDB

* Status: accepted
* Deciders: Timm Hoffmeister
* Date: 2021-02-07

Technical Story: MongoDB provides several patterns for designing data formats. This ADR provides a suggestion which one this project goes for. 

## Context and Problem Statement

MongoDB is an option for storing translation data. Translation data has to store multiple translations (max. 1 per language) for a single expression. 

Common data for an expression could look like this:
```json
{
  “tokenId”: 123,
  “label”: “cat”,
  “type”: “noun”,
  “categories”: [“animal”, “pet”],
  “complexity”: 1,
  “pictureId”: “bhasha.pictures.cat”
}
```
With one translation per language for each token, e.g.:
```json
{
  “tokenId”: 123,
  “language”: “en_UK”,
  “native”: “cat”,
  “spoken”: “cat”,
  “audio”: “bhasha.audio.en_UK.cat”
}
```

## Considered Options

MongoDB offers [multiple patterns](https://docs.mongodb.com/manual/tutorial/model-embedded-one-to-one-relationships-between-documents/) for this use-case:
* Embedded Document Pattern
* Subset Pattern

## Decision Outcome

Go for a single documents (*Embedded Document Pattern*) for better query-performance. The number of supported languages is very limited, therefore there will be hardly any writes - so the read-performances is the bottleneck of the entire system. 

Also, if required, it’s easy to change pattern. Changing this decision would require to re-write all translation data. This could be achieved via script and can be done with minimum downtime. We can read all translations from the “live” DB (accessing a replica or by throttled access) and push the data to a new MongoDB instance in the desired shape (e.g. following subset pattern).
