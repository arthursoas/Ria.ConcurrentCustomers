using Ria.ConcurrentCustomers.API.DTOs;

namespace Ria.ConcurrentCustomers.API.Storage
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Add a collection of customers to the database ordered by first and last name
        /// </summary>
        /// <param name="customers">Collection of customers</param>
        /// <returns>Collection of added customers</returns>
        ICollection<Customer> AddCustomers(ICollection<Customer> customers);

        /// <summary>
        /// Get all customers from the database
        /// </summary>
        /// <returns>Collection of stored customers</returns>
        ICollection<Customer> GetCustomers();
    }
}
