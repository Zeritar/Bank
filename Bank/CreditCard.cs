using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class CreditCard
    {
        public int CardNumber { get; set; }
        public string Fullname { get; set; }
        public int AccountNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public int CVC { get; set; }
        public int PIN { get; set; }
    }
}
