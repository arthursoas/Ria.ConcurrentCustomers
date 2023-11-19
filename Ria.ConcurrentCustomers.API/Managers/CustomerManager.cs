using Ria.ConcurrentCustomers.API.DTOs;
using Ria.ConcurrentCustomers.API.Storage;

namespace Ria.ConcurrentCustomers.API.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public ICollection<Customer> AddCustomers(ICollection<Customer> customers)
        {
            var validCustomers = customers
                .Where(c => IsCustomerValid(c))
                .ToList();

            return _customerRepository.AddCustomers(validCustomers);
        }

        public ICollection<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }

        private static bool IsCustomerValid(Customer customer)
        {
            return
                customer.AllFieldsPresent() &&
                customer.Age > 18;
        }
    }
}
