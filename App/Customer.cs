using System;

namespace App
{
    public class Customer
    {
         // TODO :  Add constructor that take Id, FirstName, Surname, Email 
         // and does validation
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public bool HasCreditLimit { get; set; }

        public int CreditLimit { get; set; }

        public Company Company { get; set; }
        public bool HasValidCreditLimit()
        {
            if (!HasCreditLimit)
                return true;
            else if (HasCreditLimit && CreditLimit < 500)
                return false;
            else
                return true;

        }
    }
}