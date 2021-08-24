using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Methods
    {
        public Data data = new Data();
        public Costumer VerifyLogin(string usernameInput, string passwordInput)
        {
            foreach (Costumer c in data.costumers)
            {
                if (usernameInput == c.Username)
                    if (passwordInput == c.Password)
                        return c;
            }
            return null;
        }

        public Costumer CreateCostumer(string usernameInput, string passwordInput, string fullNameInput)
        {
            Costumer c = new Costumer()
            {
                Username = usernameInput,
                Password = passwordInput,
                FullName = fullNameInput
            };

            data.costumers.Add(c);
            return c;
        }

        public Account CreateAccount(AccountType accountTypeInput)
        {
            //Costumer c = new Costumer()
            //{
            //    Username = usernameInput,
            //    Password = passwordInput,
            //    FullName = fullNameInput
            //};

            //data.costumers.Add(c);
            //return c;
        }

        public void CreateFakeData()
        {
            CreateCostumer("john", "1234", "John Cena");
            CreateCostumer("lars", "1234", "Lars Cena");
            CreateCostumer("torben", "1234", "Torben Cena");
        }

        public void GetAccountTypes()
        {
            data.accountTypes[0] = new AccountType() { TypeName = "Lønkonto", Fee = 2.42, InterestRate = -1 };
            data.accountTypes[1] = new AccountType() { TypeName = "Budgetkonto", Fee = 4, InterestRate = 0.01 };
            data.accountTypes[2] = new AccountType() { TypeName = "Opsparingskonto", Fee = 1, InterestRate = 0.05 };
        }
    }
}
