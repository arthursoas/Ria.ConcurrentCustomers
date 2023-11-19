using NSubstitute;
using Ria.ConcurrentCustomers.API.Controllers;
using Ria.ConcurrentCustomers.API.Managers;
using Shouldly;

namespace Ria.ConcurrentCustomers.API.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly CustomerController _customerController;
        private readonly ICustomerManager _customerManager;

        public CustomerControllerTests()
        {
            _customerManager = Substitute.For<ICustomerManager>();

            _customerController = new CustomerController(
                _customerManager);
        }

        [Fact]
        public void CreateCustomers_WhenBodyIsNotPresent_ShouldThrowException()
        {
            #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

            // Act and Assert
            var exception = Should.Throw<ArgumentException>(() => _customerController.CreateCustomers(null));
            exception.Message.ShouldBe("Customers cannot be null. Check the request body.");

            #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
