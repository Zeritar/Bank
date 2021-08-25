using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Methods
    {
        // Bankens registreringsnummer
        const string regNumber = "6666";

        // Instans af Data-klassen som håndterer listen over kunder
        public Data data = new Data();

        // Metode til at tjekke brugernavn og password for at logge ind
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

        // Metode til at oprette en ny kunde med givent brugernavn, password og fulde navn
        // og tilføje dem til listen over alle kunder
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


        // Metode til at oprette en ny konto af given type for den nuværende kunde
        // og tilføje den til kundens liste over konti
        public Account CreateAccount(Costumer currentCostumer, AccountType accountTypeInput)
        {
            Random rnd = new Random();
            Account newAccount = new Account()
            {
                AccountNumber = GenerateAccountNumber(),
                AccountType = accountTypeInput,
                Balance = 50
            };
            currentCostumer.accounts.Add(newAccount);
            return newAccount;
        }

        // Metode til at oprette et nyt kreditkort tilknyttet en given konto for den nuværende kunde
        // og tilføje det til kundens liste over kreditkort
        public CreditCard CreateCreditCard(Costumer currentCostumer, Account currentAccount)
        {
            Random rnd = new Random();
            CreditCard newCreditCard = new CreditCard()
            {
                CardNumber = GenerateCardNumber(),
                FullName = currentCostumer.FullName,
                AccountNumber = currentAccount.AccountNumber,
                ExpDate = CalculateExpiryDate(),
                CardFee = 500,
                CVC = rnd.Next(1, 999),
                PIN = rnd.Next(1, 9999)
            };
            currentAccount.Balance -= newCreditCard.CardFee;

            return newCreditCard;
        }

        // Metode til at generere et 14-cifret kontonummer som består af
        // bankens registreringsnummer + 10 tilfældige tal
        string GenerateAccountNumber()
        {
            Random rnd = new Random();
            int digit;
            string accountNumber = regNumber;

            for (int i = 0; i < 10; i++)
            {
                digit = rnd.Next(0, 10);
                accountNumber += digit.ToString();
            }
            return accountNumber;
        }


        // Metode til at genere et 16-cifret kortnummer som består af tilfældige tal
        string GenerateCardNumber()
        {
            Random rnd = new Random();
            int digit;
            string cardNumber = "";

            for (int i = 0; i < 16; i++)
            {
                digit = rnd.Next(0, 10);
                cardNumber += digit.ToString();
            }
            return cardNumber;
        }


        // Metode til at beregne en dato 5 år ud i fremtiden
        DateTime CalculateExpiryDate()
        {
            return new (DateTime.Now.Year +5, DateTime.Now.Month, DateTime.Now.Day);
        }
                
        // Metode til at udfylde listen over kontotyper
        public void GetAccountTypes()
        {
            data.accountTypes[0] = new AccountType() { TypeName = "Lønkonto", Fee = 2.42, InterestRate = -1 };
            data.accountTypes[1] = new AccountType() { TypeName = "Budgetkonto", Fee = 4, InterestRate = 1 };
            data.accountTypes[2] = new AccountType() { TypeName = "Opsparingskonto", Fee = 1, InterestRate = 5 };
        }

        public Transaction_In CreateTransaction_In(Account currentAccount, string description,
            double amount, DateTime date, double fee, string sender)
        {
            Transaction_In newTransaction_In = new Transaction_In()
            {
                Description = description,
                Amount = amount,
                Date = date,
                Fee = fee,
                Sender = sender
            };
            currentAccount.transactions_In.Add(newTransaction_In);
            return newTransaction_In;
        }

        public Transaction_Out CreateTransaction_Out(Account currentAccount, string description,
            double amount, DateTime date, double fee, string recipient)
        {
            Transaction_Out newTransaction_Out = new Transaction_Out()
            {
                Description = description,
                Amount = amount,
                Date = date,
                Fee = fee,
                Recipient = recipient
            };
            currentAccount.transactions_Out.Add(newTransaction_Out);
            return newTransaction_Out;
        }

        // Metode til at udfylde kundelisten med testdata
        public void CreateFakeData()
        {
            CreateCostumer("john", "1234", "John Cena");
            CreateCostumer("lars", "1234", "Lars Cena");
            CreateCostumer("torben", "1234", "Torben Cena");
        }
        
        // Metode til at lave overførsler
        public void CreateFakeTransactions()
        {
            for (int i = 0; i < 150; i++)
            {
                //CreateTransaction_In(string)
                //CreateTransaction_out
            }
        }
    }
}
