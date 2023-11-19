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
            var validCustomers = new List<Customer>();
            foreach (var customer in customers)
            {
                if (IsCustomerValid(customer))
                {
                    validCustomers.Add(customer);
                }
            }

            _customerRepository.AddCustomers(validCustomers);

            return validCustomers;
        }

        public ICollection<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }

        private bool IsCustomerValid(Customer customer)
        {
            if (!customer.AllFieldsPresent() || customer.Age < 18)
            {
                return false;
            }

            var storedCustomer = _customerRepository.GetCustomer(customer.Id ?? 0);
            if (storedCustomer != null)
            {
                return false;
            }

            return true;
        }
    }
}
