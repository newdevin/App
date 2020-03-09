using App.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public class CustomerService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly ICreditService creditService;
        private readonly ICustomerRepository customerRepository;

        public CustomerService()
        {
            companyRepository = new CompanyRepository();
            creditService = new CreditService();
            customerRepository = new CustomerRepository();
        }

        public CustomerService(ICompanyRepository companyRepository, ICreditService creditService, ICustomerRepository customerRepository)
        {
            this.companyRepository = companyRepository;
            this.creditService = creditService;
            this.customerRepository = customerRepository;
        }

        public bool AddCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            // TODO : Can some/all of these validations be moved into the customer class itself so that these do not get repeated
            // in other places where we need to create customer objects?
            if (!IsValidName(firstname, surname) || !IsValidEmail(email) || IsUnder21Years(dateOfBirth))
                return false;

            Customer customer = GetCustomer(firstname, surname, email, dateOfBirth, companyId);

            if (!IsValidCreditLimit(customer))
                return false;

            customerRepository.AddCustomer(customer);

            return true;
        }
        private Customer GetCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            var company = companyRepository.GetById(companyId);
            var customer = new Customer
            {
                Company = company,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstname,
                Surname = surname
            };
            return customer;
        }
        private bool IsValidCreditLimit(Customer customer)
        {
            var creditLimit = creditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);

            var company = customer.Company;
            if (company.IsVeryImportantClient())
            {
                // Skip credit check
                customer.HasCreditLimit = false;
            }
            else
            {
                customer.HasCreditLimit = true;
                if (company.IsImportantClient())
                    customer.CreditLimit = creditLimit * 2;
                else
                    customer.CreditLimit = creditLimit;
            }

            return customer.HasValidCreditLimit();


        }
        private bool IsUnder21Years(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
                return true;
            else
                return false;
        }
        private bool IsValidEmail(string email)
        {
            if (email.Contains("@") && email.Contains("."))
                return true;
            else
                return false;
        }
        private bool IsValidName(string firstname, string surname)
        {
            if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname))
                return false;
            else
                return true;
        }


    }
}
