using Ria.ConcurrentCustomers.API.DTOs;
using Ria.ConcurrentCustomers.API.Storage.Text;
using System.Text.Json;

namespace Ria.ConcurrentCustomers.API.Storage
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly object _lock = new();
        private readonly List<Customer> _customers;

        private readonly ITextCustomerStorage _textCustomerStorage;

        public CustomerRepository(
            ITextCustomerStorage textCustomerStorage)
        {
            _textCustomerStorage = textCustomerStorage;

            try
            {
                var customers = _textCustomerStorage.ReadCustomers();
                _customers = JsonSerializer.Deserialize<List<Customer>>(customers) ?? new List<Customer>();
            }
            catch
            {
                _customers = new List<Customer>();
            }
        }

        public ICollection<Customer> AddCustomers(ICollection<Customer> customers)
        {
            var addedCustomers = new List<Customer>();

            lock (_lock)
            {
                foreach (var customer in customers)
                {
                    // If customer with id is already stored
                    var added = false;
                    var storedCustomer = GetCustomer(customer.Id ?? 0);
                    if (storedCustomer != null)
                    {
                        continue;
                    }

                    // If collection is empty, add first item
                    if (_customers.Count == 0)
                    {
                        _customers.Add(customer);
                        added = true;
                    }

                    // If collection has items, check the best position to add item
                    if (!added) {
                        for (var index = 0; index < _customers.Count; index++)
                        {
                            if (!customer.GreaterThan(_customers.ElementAt(index)))
                            {
                                _customers.Insert(index, customer);
                                added = true;
                                break;
                            }
                        }
                    }

                    // If item is greater than all others in the collection, add it to the end
                    if (!added)
                    {
                        _customers.Add(customer);
                    }

                    addedCustomers.Add(customer);
                    _textCustomerStorage.SaveCustomers(_customers);
                }

                return addedCustomers;
            }
        }

        public ICollection<Customer> GetCustomers()
        {
            lock (_lock)
            {
                return _customers;
            }
        }

        private Customer? GetCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            return customer;
        }
    }
}
