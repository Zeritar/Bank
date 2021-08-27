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
        public Customer VerifyLogin
            (string usernameInput, string passwordInput)
        {
            foreach (Customer c in data.customers)
            {
                if (usernameInput == c.Username)
                    if (passwordInput == c.Password)
                        return c;
            }
            return null;
        }

        public bool VerifyUsernameInput(string usernameInput)
        {
            if (usernameInput != string.Empty)
            {
                foreach (Customer c in data.customers)
                {
                    if (usernameInput == c.Username)
                        return false;
                }
            return true;
            }
            else
            {
                return false;
            }
        }

        // Metode til at oprette en ny kunde med givent brugernavn, password og fulde navn
        // og tilføje dem til listen over alle kunder
        public Customer CreateCustomer(string usernameInput, string passwordInput, string fullNameInput)
        {
            Customer c = new Customer()
            {
                Username = usernameInput,
                Password = passwordInput,
                FullName = fullNameInput
            };

            data.customers.Add(c);
            return c;
        }


        // Metode til at oprette en ny konto af given type for den nuværende kunde
        // og tilføje den til kundens liste over konti
        public Account CreateAccount(Customer currentCustomer, AccountType accountTypeInput)
        {
            Random rnd = new Random();
            Account newAccount = new Account()
            {
                AccountNumber = GenerateAccountNumber(),
                AccountType = accountTypeInput,
                Balance = 50
            };
            currentCustomer.accounts.Add(newAccount);
            return newAccount;
        }

        // Metode til at oprette et nyt kreditkort tilknyttet en given konto for den nuværende kunde
        // og tilføje det til kundens liste over kreditkort
        public CreditCard CreateCreditCard(Customer currentCustomer, Account currentAccount)
        {
            Random rnd = new Random();
            CreditCard newCreditCard = new CreditCard()
            {
                CardNumber = GenerateCardNumber(),
                FullName = currentCustomer.FullName,
                AccountNumber = currentAccount.AccountNumber,
                ExpDate = CalculateExpiryDate(),
                CardFee = 500,
                CVC = rnd.Next(1, 999),
                PIN = rnd.Next(1, 9999)
            };
            currentAccount.Balance -= newCreditCard.CardFee;

            currentCustomer.creditCards.Add(newCreditCard);

            return newCreditCard;
        }

        // Metode til at generere et 14-cifret kontonummer som består af
        // bankens registreringsnummer + 10 tilfældige tal
        string GenerateAccountNumber()
        {
            Random rnd = new Random();
            int digit;
            string accountNumber = regNumber + " ";

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
            return new(DateTime.Now.Year + 5, DateTime.Now.Month, DateTime.Now.Day);
        }

        // Metode til at udfylde listen over kontotyper
        public void GetAccountTypes()
        {
            data.accountTypes[0] = new AccountType() { TypeName = "Lønkonto", Fee = 2.42, InterestRate = -1 };
            data.accountTypes[1] = new AccountType() { TypeName = "Budgetkonto", Fee = 4, InterestRate = 1 };
            data.accountTypes[2] = new AccountType() { TypeName = "Opsparingskonto", Fee = 1, InterestRate = 5 };
        }

        public List<Transaction_View> GetAllTransactions(Account currentAccount)
        {
            List<Transaction_View> t_view = new List<Transaction_View>();

            foreach (Transaction_In t_in in currentAccount.transactions_In)
            {
                t_view.Add(new Transaction_View()
                {
                    Date = t_in.Date,
                    Description = t_in.Description,
                    Amount = t_in.Amount
                });
            }

            foreach (Transaction_Out t_out in currentAccount.transactions_Out)
            {
                t_view.Add(new Transaction_View()
                {
                    Date = t_out.Date,
                    Description = t_out.Description,
                    Amount = -t_out.Amount
                });
            }

            return t_view;
        }

        public List<Transaction_View> SortTransactionsByDate(List<Transaction_View> transactions)
        {
            transactions.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
            return transactions;
        }

        public Transaction_In CreateTransaction_In(Account currentAccount, string description,
            double amount, DateTime date, bool fee, string sender)
        {
            Transaction_In newTransaction_In = new Transaction_In()
            {
                Description = description,
                Amount = amount,
                Date = date,
                Sender = sender
            };

            if (fee)
            {
                newTransaction_In.Fee = currentAccount.AccountType.Fee;
            }
            else
            {
                newTransaction_In.Fee = 0;
            }

            currentAccount.Balance += amount;
            currentAccount.Balance -= newTransaction_In.Fee;
            currentAccount.transactions_In.Add(newTransaction_In);
            return newTransaction_In;
        }
        
        
        // Metode til at lave de udgånde transactioner samt balangse
        public Transaction_Out CreateTransaction_Out(Account currentAccount, string description,
            double amount, DateTime date, bool fee, string recipient)
        {
            Transaction_Out newTransaction_Out = new Transaction_Out()
            {
                Description = description,
                Amount = amount,
                Date = date,
                Recipient = recipient
            };

            if (fee)
            {
                newTransaction_Out.Fee = currentAccount.AccountType.Fee;
            }
            else
            {
                newTransaction_Out.Fee = 0;
            }

            currentAccount.Balance -= amount;
            currentAccount.Balance -= newTransaction_Out.Fee;
            currentAccount.transactions_Out.Add(newTransaction_Out);
            return newTransaction_Out;
        }

        // Metode til at udskive den samlede Transactions liste
        public void WriteViewEntry(Transaction_View t_view)
        {
            int posY = Console.GetCursorPosition().Top + 1;
            Console.SetCursorPosition(2, posY);
            Console.Write($"{t_view.Date.Day.ToString().PadLeft(2, '0')}/{t_view.Date.Month.ToString().PadLeft(2, '0')}/{t_view.Date.Year}");
            Console.SetCursorPosition(14, posY);
            Console.Write($"{t_view.Description}");

            string amountString = t_view.Amount.ToString("0.00") + " kr.";

            for (int i = 0; i < amountString.Length; i++)
            {
                Console.SetCursorPosition(60 - i, posY);
                Console.Write(amountString[amountString.Length - 1 - i]);
            }

            //Console.SetCursorPosition(50, posY);
            //Console.Write($"{t_view.Amount} kr.");
        }

        

        public string GetHiddenPasswordInput()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    // Slet en karakter fra konsolvinduet
                    Console.Write("\b \b");

                    // Træk sidste karakter fra password
                    // 0 er index start, '..' betyder range, '^' betyder fromEnd: true
                    // 1 betyder index 1, men fra højre pga. fromEnd.
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }

        public void GenerateMainMenu(Customer currentCustomer, CustomerStatus customerStatus, bool invalidChoice)
        {
            Console.WriteLine("Vælg et menupunkt:\n");
            switch (customerStatus)
            {
                case CustomerStatus.HasAccount:
                    Console.WriteLine("1) Se konti");
                    Console.WriteLine("2) Opret ny konto");
                    Console.WriteLine("3) Opret nyt kort");
                    Console.WriteLine("4) Opret overførsel");
                    Console.WriteLine("5) Log ud");
                    break;
                case CustomerStatus.HasCreditCard:
                    Console.WriteLine("1) Se kort");
                    Console.WriteLine("2) Opret ny konto");
                    Console.WriteLine("3) Opret nyt kort");
                    Console.WriteLine("4) Log ud");
                    break;
                case CustomerStatus.HasNeither:
                    Console.WriteLine("1) Opret ny konto");
                    Console.WriteLine("2) Log ud");
                    break;
                case CustomerStatus.HasBoth:
                    Console.WriteLine("1) Se konti");
                    Console.WriteLine("2) Se kort");
                    Console.WriteLine("3) Opret ny konto");
                    Console.WriteLine("4) Opret nyt kort");
                    Console.WriteLine("5) Opret overførsel");
                    Console.WriteLine("6) Log ud");
                    break;
            }
            // Hvis bruger trykkede på en ugyldig tast ved sidste tjek
            if (invalidChoice)
            {
                int curPosY = Console.GetCursorPosition().Top;
                Console.SetCursorPosition(0, curPosY + 2);
                Console.WriteLine("Vælg et gyldigt menupunkt.");
            }

        }

        public string HandleMainMenuChoice(CustomerStatus customerStatus, int choice)
        {
            string returnedChoice = "none";

            switch (customerStatus)
            {
                case CustomerStatus.HasAccount:
                    switch (choice)
                    {
                        case 1:
                            // Gå til menu for at vælge konti
                            returnedChoice = "viewAccounts";
                            break;
                        case 2:
                            // Gå til menu for oprettelse af en ny konto
                            returnedChoice = "createAccount";
                            break;
                        case 3:
                            // Gå til menu for oprettelse af et nyt kreditkort
                            returnedChoice = "createCreditCard";
                            break;
                        case 4:
                            // Opret overførsel
                            returnedChoice = "createTransaction";
                            break;
                        case 5:
                            // Log ud
                            returnedChoice = "logout";
                            break;
                        default:
                            // Bruger har ikke valgt en gyldig mulighed
                            returnedChoice = "none";
                            break;
                    }
                    break;
                    // Edge case. Burde ikke have mulighed for at oprette et kreditkort uden at have en konto
                    // Er kun hvis fake data bliver oprettet hvor bruger ikke har en konto, men har et kreditkort
                case CustomerStatus.HasCreditCard:
                    switch (choice)
                    {
                        case 1:
                            // Gå til visning af kreditkort
                            returnedChoice = "viewCreditCards";
                            break;
                        case 2:
                            // Gå til menu for oprettelse af en ny konto
                            returnedChoice = "createAccount";
                            break;
                        case 3:
                            // Gå til menu for oprettelse af et nyt kreditkort
                            returnedChoice = "createCreditCard";
                            break;
                        case 4:
                            // Log ud
                            returnedChoice = "logout";
                            break;
                        default:
                            // Bruger har ikke valgt en gyldig mulighed
                            returnedChoice = "none";
                            break;
                    }
                    break;
                case CustomerStatus.HasNeither:
                    switch (choice)
                    {
                        case 1:
                            // Gå til menu for at vælge konti
                            returnedChoice = "createAccount";
                            break;
                        case 2:
                            // Gå til menu for oprettelse af en ny konto
                            returnedChoice = "logout";
                            break;
                        default:
                            // Bruger har ikke valgt en gyldig mulighed
                            returnedChoice = "none";
                            break;
                    }
                    break;
                case CustomerStatus.HasBoth:
                    switch (choice)
                    {
                        case 1:
                            // Gå til menu for at vælge konti
                            returnedChoice = "viewAccounts";
                            break;
                        case 2:
                            // Gå til visning af kreditkort
                            returnedChoice = "viewCreditCards";
                            break;
                        case 3:
                            // Gå til menu for oprettelse af en ny konto
                            returnedChoice = "createAccount";
                            break;
                        case 4:
                            // Gå til menu for oprettelse af et nyt kreditkort
                            returnedChoice = "createCreditCard";
                            break;
                        case 5:
                            // Opret overførsel
                            returnedChoice = "createTransaction";
                            break;
                        case 6:
                            // Log ud
                            returnedChoice = "logout";
                            break;
                        default:
                            // Bruger har ikke valgt en gyldig mulighed
                            returnedChoice = "none";
                            break;
                    }
                    break;
            }

            return returnedChoice;
        }

        // Metode til at udfylde kundelisten med testdata
        public void CreateFakeData()
        {
            CreateCustomer("john", "1234", "John Cena");
            CreateCustomer("lars", "1234", "Lars Cena");
            CreateCustomer("torben", "1234", "Torben Cena");

            foreach (Customer currentCustomer in data.customers)
            {
                Account currentAccount = CreateAccount(currentCustomer, data.accountTypes[1]);

                for (int i = 0; i < 5; i++)
                {
                    Random rnd = new Random();
                    double amoin = rnd.Next(0, 25000);
                    double amoout = rnd.Next(0, 25000);
                    int hasFee = rnd.Next(0, 1);
                    double fee = 0;
                    if (hasFee == 1)
                    {
                        fee = currentAccount.AccountType.Fee;
                    }
                    CreateTransaction_In(currentAccount, "Fake Data", amoin, GenerateRandomDate(), false, GenerateCardNumber());
                    CreateTransaction_Out(currentAccount, "Fake News", amoout, GenerateRandomDate(), true, GenerateAccountNumber());
                }
            }
            CreateCustomer("uden", "1234", "Kunde uden konto");
            Customer tesCustomertWithAccount = CreateCustomer("med", "1234", "Kunde med konto og kort");
            Account testAccount = CreateAccount(tesCustomertWithAccount, data.accountTypes[0]);
            CreateCreditCard(tesCustomertWithAccount, testAccount);
            CreateCreditCard(tesCustomertWithAccount, testAccount);
        }

        // Metode til at generere en tilfældig dato
        public DateTime GenerateRandomDate()
        {
            DateTime start = new DateTime(1970, 1, 1);
            Random gen = new Random();
            int range = (int)(DateTime.Today - start).TotalDays;

            return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }

        public enum CustomerStatus
        {
            HasAccount,
            HasCreditCard,
            HasNeither,
            HasBoth
        }
    }
}
