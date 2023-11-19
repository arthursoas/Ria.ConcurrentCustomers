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
                            if (!customer.GraterThan(_customers.ElementAt(index)))
                            {
                                var addAtIndex = index - 1 < 0 ? 0 : index - 1;
                                _customers.Insert(addAtIndex, customer);
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
