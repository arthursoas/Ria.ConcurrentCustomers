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

        public bool GraterThan(Customer other)
        {
            var lastNameComparsion = string.Compare(LastName, other.LastName, StringComparison.OrdinalIgnoreCase);
            if (lastNameComparsion != 0)
            {
                return lastNameComparsion > 0;
            }

            var firstNameComparsion = string.Compare(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase);
            return firstNameComparsion > 0;
        }
    }
}
