using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Customer
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public List<Account> accounts = new List<Account>();
        public List<CreditCard> creditCards = new List<CreditCard>();
    }
}
