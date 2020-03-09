using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public interface ICreditService
    {
        int GetCreditLimit(string firstname, string surname, DateTime dateOfBirth);
    }

    public class CreditService : ICreditService
    {
        public int GetCreditLimit(string firstname, string surname, DateTime dateOfBirth)
        {
            using (var customerCreditService = new CustomerCreditServiceClient())
            {
                return customerCreditService.GetCreditLimit(firstname, surname, dateOfBirth);
            }
        }

    }


}
