namespace Ria.ConcurrentCustomers.API.DTOs
{
    public class Customer
    {
        public int? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Age { get; set; }

        public bool AllFieldsPresent()
        {
            return !(
                Id == null ||
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                Age == null
            );
        }
    }
}
