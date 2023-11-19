# 🔢 Concurrent Customers

This projetc is part of [Ria Money Transfer](https://www.riamoneytransfer.com/en-us/) Software Engineer assignment.

## Problem

A server that adds each customer as an object to an internal collection. The customers will not be appended to the collection but instead it will be inserted at a position so that the customers are sorted by their last names and then first names WITHOUT using any available sorting functionality.

The server also persists the collection so it will be still available after a restart of the server.

## Technologies used

- `C#` as programming language

## Solution

An .NET API was created with two endpoints:

1. **Get Customers**
```bash
curl -X 'GET' \
  'https://localhost:7005/customers' \
  -H 'accept: text/plain'
```

2. **Add Customers**
```bash
curl -X 'POST' \
  'https://localhost:7005/customers' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '[
  {
    "id": 0,
    "firstName": "string",
    "lastName": "string",
    "age": 0
  }
]'
```

There is no limit of how many customers can be added at the same request.

The customers are stored in memory, and in a text file available at `
