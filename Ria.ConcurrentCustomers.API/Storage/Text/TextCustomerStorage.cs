using Ria.ConcurrentCustomers.API.DTOs;
using System.Text.Json;
using System.Text;

namespace Ria.ConcurrentCustomers.API.Storage.Text
{
    public class TextCustomerStorage : ITextCustomerStorage
    {
        private readonly string _filePath = "./Storage/Text/Customers.txt";

        public string ReadCustomers()
        {
            using var reader = new StreamReader(_filePath);

            return reader.ReadToEnd();
        }

        public void SaveCustomers(ICollection<Customer> customers)
        {
            using var fs = new FileStream(_filePath, FileMode.Truncate, FileAccess.Write);
            using var sw = new StreamWriter(fs, Encoding.UTF8, bufferSize: 4096, leaveOpen: true);

            var customersString = JsonSerializer.Serialize(customers);
            sw.WriteLine(customersString);
            sw.Flush();
        }
    }
}
