using Ria.ConcurrentCustomers.API.DTOs;

namespace Ria.ConcurrentCustomers.API.Storage
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly object _lock = new();
        private readonly List<Customer> _customers;

        public CustomerRepository()
        {
            _customers = new List<Customer>();
        }

        public void AddCustomers(ICollection<Customer> customers)
        {
            lock (_lock)
            {
                _customers.AddRange(customers);
            }
        }

        public ICollection<Customer> GetCustomers()
        {
            lock (_lock)
            {
                return _customers;
            }
        }

        public Customer? GetCustomer(int id)
        {
            lock (_lock)
            {
                var customer = _customers.FirstOrDefault(c => c.Id == id);
                return customer;
            }
        }
    }
}
