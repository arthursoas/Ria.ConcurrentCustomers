using NSubstitute;
using Ria.ConcurrentCustomers.API.DTOs;
using Ria.ConcurrentCustomers.API.Managers;
using Ria.ConcurrentCustomers.API.Storage;
using Shouldly;

namespace Ria.ConcurrentCustomers.API.Tests.Managers
{
    public class CustomerManagerTests
    {
        private readonly ICustomerManager _customerManager;
        private readonly ICustomerRepository _customerRepository;

        public CustomerManagerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();

            _customerManager = new CustomerManager(
                _customerRepository);
        }

        [Fact]
        public void AddCustomers_WhenAllCustomerAreValid_ShouldAddAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 18 },
            };

            // Act
            _customerManager.AddCustomers(customers);

            // Assert
            _customerRepository
                .Received(1)
                .AddCustomers(Arg.Is<ICollection<Customer>>(c =>
                    c.Count == 3));
        }

        [Fact]
        public void AddCustomers_WhenSomeCustomerDoesNotHas18YearsOld_ShouldNotStorethisCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 10 },
            };

            // Act
            _customerManager.AddCustomers(customers);

            // Assert
            _customerRepository
                .Received(1)
                .AddCustomers(Arg.Is<ICollection<Customer>>(c =>
                    c.Count == 2 &&
                    !c.Any(c => c.FirstName == "Victor")));
        }

        [Fact]
        public void AddCustomers_WhenSomeCustomerIsNotValid_ShouldNotStorethisCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 1, LastName = "Zaquiel", Age = 18 },
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 18 },
            };

            // Act
            _customerManager.AddCustomers(customers);

            // Assert
            _customerRepository
                .Received(1)
                .AddCustomers(Arg.Is<ICollection<Customer>>(c =>
                    c.Count == 2 &&
                    !c.Any(c => c.LastName == "Zaquiel")));
        }

        [Fact]
        public void GetCustomers_ShouldGetCustomersFromRepository()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer{ Id = 2, FirstName = "Arthur", LastName = "Balbo", Age = 18 },
                new Customer{ Id = 3, FirstName = "Victor", LastName = "Drew", Age = 10 },
                new Customer{ Id = 1, FirstName = "Lucas", LastName = "Zaquiel", Age = 18 },
            };
            _customerRepository.GetCustomers().Returns(customers);

            // Act
            var storedCustomers = _customerManager.GetCustomers();

            // Assert
            storedCustomers.ShouldBe(customers);
        }
    }
}
