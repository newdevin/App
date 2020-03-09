using App.Repositories;
using NSubstitute;
using System;
using Xunit;

namespace App.Tests
{
    public class CustomerServiceTest
    {
        ICompanyRepository companyRepository;
        ICreditService creditService;
        ICustomerRepository customerRepository;

        public CustomerServiceTest()
        {
            companyRepository = Substitute.For<ICompanyRepository>();
            creditService = Substitute.For<ICreditService>();
            customerRepository = Substitute.For<ICustomerRepository>();
        }
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldReturnFalseWhenFirstnameIsEmptyOrNull(string firstName)
        {
            CustomerService customerService = new CustomerService();
            var result = customerService.AddCustomer(firstName, "surname", "some@email.com", new DateTime(2000, 01, 01), 1);
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldReturnFalseWhenSurnameIsEmptyOrNull(string surname)
        {
            CustomerService customerService = new CustomerService();
            var result = customerService.AddCustomer("firstname", surname, "some@email.com", new DateTime(2000, 01, 01), 1);
            Assert.False(result);
        }

        [Theory]
        [InlineData("abc.com")]
        [InlineData("adc@a")]
        public void ShouldReturnFalseWhenEmailIsInvalid(string email)
        {
            CustomerService customerService = new CustomerService();
            var result = customerService.AddCustomer("firstname", "surname", email, new DateTime(2000, 01, 01), 1);
            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseWhenCustomerIsUnder21()
        {
            var dateOfBirth = DateTime.Now.AddYears(-20);
            CustomerService customerService = new CustomerService();
            var result = customerService.AddCustomer("firstname", "surname", "some@email.com", dateOfBirth, 1);
            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnFalseIfCompanyIsNeitherVeryImportantNorImportantAndLimitIs100()
        {
            var companyId = 4;
            var firstName = "Joe";
            var surname = "Bloggs";
            var email = "joe.bloggs@adomain.com";
            var dateOfBirth = new DateTime(1980, 3, 27);

            var company = new Company { Id = 1, Name = "SomeCompany", Classification = Classification.Bronze };
            
            companyRepository.GetById(companyId).Returns(company);
            creditService.GetCreditLimit(firstName, surname, dateOfBirth).Returns(100);


            CustomerService customerService = new CustomerService(companyRepository, creditService, customerRepository);
            var result = customerService.AddCustomer(firstName, surname, email, dateOfBirth, companyId);

            Assert.False(result);

        }

        [Fact]
        public void ShouldReturnTrueIfCompanyIsVeryImportantClient()
        {
            var companyId = 4;
            var firstName = "Joe";
            var surname = "Bloggs";
            var email = "joe.bloggs@adomain.com";
            var dateOfBirth = new DateTime(1980, 3, 27);

            var company = new Company { Id = 1, Name = "VeryImportantClient", Classification = Classification.Bronze };

            companyRepository.GetById(companyId).Returns(company);
            creditService.GetCreditLimit(firstName, surname, dateOfBirth).Returns(100);


            CustomerService customerService = new CustomerService(companyRepository, creditService, customerRepository);
            var result = customerService.AddCustomer(firstName, surname, email, dateOfBirth, companyId);

            Assert.True(result);
        }


        [Fact]
        public void ShouldReturnTrueIfCompanyIsImportantClientAndCreditLimitOver300()
        {
            var companyId = 4;
            var firstName = "Joe";
            var surname = "Bloggs";
            var email = "joe.bloggs@adomain.com";
            var dateOfBirth = new DateTime(1980, 3, 27);

            var company = new Company { Id = 1, Name = "VeryImportantClient", Classification = Classification.Bronze };

            companyRepository.GetById(companyId).Returns(company);
            creditService.GetCreditLimit(firstName, surname, dateOfBirth).Returns(300);


            CustomerService customerService = new CustomerService(companyRepository, creditService, customerRepository);
            var result = customerService.AddCustomer(firstName, surname, email, dateOfBirth, companyId);

            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnTrueIfCompanyIsNeitherVeryImportantNotImportantAndCreditLimitIs500()
        {
            var companyId = 4;
            var firstName = "Joe";
            var surname = "Bloggs";
            var email = "joe.bloggs@adomain.com";
            var dateOfBirth = new DateTime(1980, 3, 27);

            var company = new Company { Id = 1, Name = "VeryImportantClient", Classification = Classification.Bronze };

            companyRepository.GetById(companyId).Returns(company);
            creditService.GetCreditLimit(firstName, surname, dateOfBirth).Returns(500);


            CustomerService customerService = new CustomerService(companyRepository, creditService, customerRepository);
            var result = customerService.AddCustomer(firstName, surname, email, dateOfBirth, companyId);

            Assert.True(result);
        }

    }
}
