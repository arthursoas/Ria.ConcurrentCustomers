using Ria.ConcurrentCustomer.Stresser;

var stresser = new Stresser("https://localhost:7005");

await stresser.StressCustomersAPI(100);
Console.ReadKey();