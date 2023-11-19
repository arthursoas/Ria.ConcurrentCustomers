using Ria.ConcurrentCustomers.API.DTOs;

namespace Ria.ConcurrentCustomers.API.Storage.Text
{
    public interface ITextCustomerStorage
    {
        /// <summary>
        /// Read customers saved on text file
        /// </summary>
        string ReadCustomers();

        /// <summary>
        /// Saves customers in a text file
        /// </summary>
        /// <param name="customers">Collection of customers</param>
        void SaveCustomers(ICollection<Customer> customers);
    }
}
