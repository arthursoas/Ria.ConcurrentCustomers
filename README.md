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
Added customers are returned on the response body. Customers that do not comply with the requirements are not added to the storage.

---

The customers are stored in memory and in a text file, available at the path `/Ria.ConcurrentCustomers.API/Storage/Text/Customers.txt`.

To avoid using sorting methods, while a new customer is being stored, he/she is inserted at the collection in the correct position, according to an alphabetical order algorithm.

The statement statement [lock](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock) is being used to avoid concurrency while customers from different requests are being added.

## Tests

The solution has a high unit test coverage, and also the project `Ria.ConcurrentCustomers.Stresser` can be used to test the accuracy and performance of the API.

The Stresser must be run only after the API is online.

The file storage also must be empty to ensure the accuracy of the tests, as customers with repeated IDs are not allowed on the API.
