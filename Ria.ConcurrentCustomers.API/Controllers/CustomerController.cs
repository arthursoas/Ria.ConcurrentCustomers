using Microsoft.AspNetCore.Mvc;
using Ria.ConcurrentCustomers.API.DTOs;
using Ria.ConcurrentCustomers.API.Managers;

namespace Ria.ConcurrentCustomers.API.Controllers
{
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerManager _customerManager;

        public CustomerController(
            ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        [HttpPost]
        public ICollection<Customer> CreateCustomers([FromBody] ICollection<Customer> customers)
        {
            if (customers == null)
            {
                throw new ArgumentException("Customers cannot be null. Check the request body.");
            }

            return _customerManager.AddCustomers(customers);
        }

        [HttpGet]
        public ICollection<Customer> GetCustomers()
        {
            return _customerManager.GetCustomers();
        }
    }
}
