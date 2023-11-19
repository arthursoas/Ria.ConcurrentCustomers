using NSubstitute;
using Ria.ConcurrentCustomers.API.DTOs;
using Ria.ConcurrentCustomers.API.Storage;
using Ria.ConcurrentCustomers.API.Storage.Text;
using Shouldly;

namespace Ria.ConcurrentCustomers.API.Tests.Storage
{
    public class CustomerRepositoryTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITextCustomerStorage _textCustomerStorage;

        public CustomerRepositoryTests()
        {
            _textCustomerStorage = Substitute.For<ITextCustomerStorage>();
            _textCustomerStorage
                .ReadCustomers()
                .Returns(string.Empty);

            _customerRepository = new CustomerRepository(
                _textCustomerStorage);
        }

        [Fact]
        public void AddCustomers_WhenCustomerAreValid_ShouldOrderThemByLastName()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 18 },
            };
            var orderedCustomers = customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName);

            // Act
            _customerRepository.AddCustomers(customers);

            // Assert
            var storedCustomers = _customerRepository.GetCustomers();

            storedCustomers.ShouldBe(orderedCustomers);
        }

        [Fact]
        public void AddCustomers_WhenThereAreEqualLastNames_ShouldOrderThemByFirstName()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Drew", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 18 },
            };
            var orderedCustomers = customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName);

            // Act
            _customerRepository.AddCustomers(customers);

            // Assert
            var storedCustomers = _customerRepository.GetCustomers();

            storedCustomers.ShouldBe(orderedCustomers);
        }

        [Fact]
        public void AddCustomers_WhenCustomersHasRepeatedId_ShouldIgnoreIt()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 2, FirstName = "Victor", LastName = "Drew", Age = 18 },
            };

            // Act
            var addedCustomers = _customerRepository.AddCustomers(customers);

            // Assert
            var storedCustomers = _customerRepository.GetCustomers();

            addedCustomers.Count.ShouldBe(2);
            storedCustomers.Count.ShouldBe(2);
            storedCustomers.ShouldNotContain(c => c.FirstName == "Victor" && c.LastName == "Drew");
        }

        [Fact]
        public async Task AddCustomers_WhenTwoTaskRunAtTheSameTime_ShouldNotConflict()
        {
            // Arrange
            var customers1 = new List<Customer>
            {
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 18 },
            };
            var customers2 = new List<Customer>
            {
                new Customer{ Id = 4, FirstName = "Alek", LastName = "Chan", Age = 18 },
                new Customer{ Id = 5, FirstName = "Karen", LastName = "Powell", Age = 18 },
                new Customer{ Id = 6, FirstName = "Mark", LastName = "Drew", Age = 18 },
            };
            var orderedCustomers = customers1
                .Concat(customers2)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName);

            var tasks = new List<Task<ICollection<Customer>>> {
                Task.Factory.StartNew(() => _customerRepository.AddCustomers(customers1)),
                Task.Factory.StartNew(() => _customerRepository.AddCustomers(customers2))
            };

            // Act
            await Task.WhenAll(tasks);

            // Assert
            var storedCustomers = _customerRepository.GetCustomers();

            storedCustomers.Count.ShouldBe(6);
            storedCustomers.ShouldBe(orderedCustomers);
        }
    }
}
