using Ria.ConcurrentCustomers.API.DTOs;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Ria.ConcurrentCustomer.Stresser
{
    public class Stresser
    {
        private readonly string[] _firstNames = {
            "Leia", "Sadie", "Jose", "Sara",
            "Frank", "Dewey", "Tomas", "Joel",
            "Lukas", "Carlos"
        };
        private readonly string[] _lastNames = {
            "Liberty", "Ray", "Harrison", "Ronan",
            "Drew", "Powell", "Larsen", "Chan",
            "Anderson", "Lane"
        };

        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public Stresser(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

        public async Task<bool> StressCustomersAPI(int count)
        {
            var random = new Random();
            var timer = new Stopwatch();

            var requests = new List<Task>();
            var allCustomers = new List<Customer>();

            var customerId = 0;

            timer.Start();
            for (var index = 0; index < count; index++)
            {
                var customersCount = random.Next(2, 10);
                var customers = new List<Customer>(customersCount);
                for (var customerIndex = 0; customerIndex < customersCount; customerIndex++)
                {
                    customers.Add(new Customer
                    {
                        Id = customerId,
                        FirstName = _firstNames[random.Next(0, _firstNames.Length)],
                        LastName = _lastNames[random.Next(0, _lastNames.Length)],
                        Age = random.Next(0, 91)
                    });
                    customerId++;
                }

                requests.Add(PostAsync(customers));

                var shouldGetRandomly = random.Next(0, 2) == 0 ? true : false;
                if (shouldGetRandomly)
                {
                    requests.Add(GetAsync());
                }

                allCustomers.AddRange(customers);
            }

            Task.WaitAll(requests.ToArray());
            timer.Stop();

            var storedCustomers = await GetAsync();
            var validCustomers = allCustomers
                .Where(c => c.Age >= 18)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();

            var success = CheckResponseIsCorrect(storedCustomers, validCustomers);
            OutputResults(success, storedCustomers, requests, timer);

            return success;
        }

        private Task PostAsync(ICollection<Customer> customers)
        {
            var body = JsonSerializer.Serialize(customers);
            var postMessage = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/customers")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            return _httpClient.SendAsync(postMessage);
        }

        private async Task<ICollection<Customer>> GetAsync()
        {
            var getMessage = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/customers");
            var response = await _httpClient.SendAsync(getMessage);

            return JsonSerializer.Deserialize<List<Customer>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }

        private static bool CheckResponseIsCorrect(ICollection<Customer> storedCustomers, ICollection<Customer> expectedCustomers)
        {
            if (storedCustomers.Count != expectedCustomers.Count)
            {
                return false;
            }

            foreach (var storedCustomer in storedCustomers)
            {
                var expectedCustomer = expectedCustomers.FirstOrDefault(c =>
                    storedCustomer.Id == c.Id &&
                    storedCustomer.FirstName == c.FirstName &&
                    storedCustomer.LastName == c.LastName &&
                    storedCustomer.Age == c.Age);

                if (expectedCustomer == null)
                {
                    return false;
                }
            }

            return true;
        }

        private static void OutputResults(bool success, ICollection<Customer> storedCustomers, ICollection<Task> requests, Stopwatch timer)
        {
            Console.WriteLine($"Completed in {timer.ElapsedMilliseconds}ms\n");

            if (success)
            {
                Console.WriteLine("Success ✅");
                Console.WriteLine($"Request Sent to the API: {requests.Count}");
                Console.WriteLine($"Customers created: {storedCustomers.Count}");
            }
            else
            {
                Console.WriteLine("Failure ❌");
                Console.WriteLine($"Request Sent to the API: {requests.Count}");
                Console.WriteLine($"Customers created: {storedCustomers.Count}");
                Console.WriteLine($"Expected customers to be created: {requests.Count}");
                Console.WriteLine("\nHave you cleaned data from the previous test?");
            }

            Console.WriteLine($"\nEach customer was checked to be sure they were created correctly.");
        }
    }
}
