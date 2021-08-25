using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class CreditCard
    {
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public string AccountNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public double CardFee { get; set; }
        public int CVC { get; set; }
        public int PIN { get; set; }
    }
}
