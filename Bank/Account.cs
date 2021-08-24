using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Account
    {

        public int AccountNumber { get; set; }
        public string AccountName { get; set; }
        public double Balance { get; set; }
        // Kan fjernes og læses fra account type
        public double InterestRate { get; set; }

        public List<Transaction_In> transactions_In = new List<Transaction_In>();
        public List<Transaction_Out> transactions_Out = new List<Transaction_Out>();
    }
}
