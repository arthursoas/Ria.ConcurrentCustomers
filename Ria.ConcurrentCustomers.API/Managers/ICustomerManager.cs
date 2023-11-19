using Ria.ConcurrentCustomers.API.DTOs;

namespace Ria.ConcurrentCustomers.API.Managers
{
    public interface ICustomerManager
    {
        /// <summary>
        /// Validates customers and add the valid ones to the database
        /// </summary>
        /// <param name="customers">Collection of customers</param>
        /// <returns>Collection of added customers</returns>
        ICollection<Customer> AddCustomers(ICollection<Customer> customers);

        /// <summary>
        /// Get the customers from the database
        /// </summary>
        /// <returns>Collection of stored customers</returns>
        ICollection<Customer> GetCustomers();
    }
}
