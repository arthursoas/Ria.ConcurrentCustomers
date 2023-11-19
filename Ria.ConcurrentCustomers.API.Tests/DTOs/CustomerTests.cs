using Ria.ConcurrentCustomers.API.DTOs;
using Shouldly;

namespace Ria.ConcurrentCustomers.API.Tests.DTOs
{
    public class CustomerTests
    {
        [Fact]
        public void AllFieldsPresent_WhenAllFieldsArePresent_ShouldReturnTrue()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Carlos",
                LastName = "Anderson",
                Age = 18
            };

            // Act
            var valid = customer.AllFieldsPresent();

            // Assert
            valid.ShouldBeTrue();
        }

        [Fact]
        public void AllFieldsPresent_WhenSomeFieldsAreMissing_ShouldReturnFalse()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Carlos",
                LastName = "Anderson",
                Age = 18
            };

            // Act
            var valid = customer.AllFieldsPresent();

            // Assert
            valid.ShouldBeFalse();
        }

        [Fact]
        public void GraterThan_WhenLastNameIsGreater_ShouldReturnTrue()
        {
            // Arrange
            var customer1 = new Customer { FirstName = "Carlos", LastName = "Liberty" };
            var customer2 = new Customer { FirstName = "Leia", LastName = "Anderson" };

            // Act
            var greater = customer1.GreaterThan(customer2);

            // Assert
            greater.ShouldBeTrue();
        }

        [Fact]
        public void GraterThan_WhenLastNameIsNotGreater_ShouldReturnFalse()
        {
            // Arrange
            var customer1 = new Customer { FirstName = "Carlos", LastName = "Harrison" };
            var customer2 = new Customer { FirstName = "Leia", LastName = "Liberty" };

            // Act
            var greater = customer1.GreaterThan(customer2);

            // Assert
            greater.ShouldBeFalse();
        }

        [Fact]
        public void GraterThan_WhenLastNamesAreEqualAndFirstNameIsGreater_ShouldReturnTrue()
        {
            // Arrange
            var customer1 = new Customer { FirstName = "Jose", LastName = "Harrison" };
            var customer2 = new Customer { FirstName = "Dewey", LastName = "Harrison" };

            // Act
            var greater = customer1.GreaterThan(customer2);

            // Assert
            greater.ShouldBeTrue();
        }

        [Fact]
        public void GraterThan_WhenLastNamesAreEqualAndFirstNameIsNotGreater_ShouldReturnFalse()
        {
            // Arrange
            var customer1 = new Customer { FirstName = "Jose", LastName = "Harrison" };
            var customer2 = new Customer { FirstName = "Sara", LastName = "Harrison" };

            // Act
            var greater = customer1.GreaterThan(customer2);

            // Assert
            greater.ShouldBeFalse();
        }

        [Fact]
        public void GraterThan_WhenBothLastAndFirstNamesAreEqual_ShouldReturnFalse()
        {
            // Arrange
            var customer1 = new Customer { FirstName = "Sara", LastName = "Harrison" };
            var customer2 = new Customer { FirstName = "Sara", LastName = "Harrison" };

            // Act
            var greater = customer1.GreaterThan(customer2);

            // Assert
            greater.ShouldBeFalse();
        }
    }
}
