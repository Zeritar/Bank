using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Menus
    {
        Methods methods = new Methods();
        
        public void Welcome()
        {
            methods.CreateFakeData();
            methods.GetAccountTypes();

            Console.Clear();
            Console.WriteLine("=Velkommen til Cirkel Bank=");
            Console.WriteLine("Vælg et menupunkt:\n");
            Console.WriteLine("1) Login");
            Console.WriteLine("2) Opret bruger");
            Console.WriteLine("3) Afslut programmet");
            bool validChar = false;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case '1':
                        validChar = true;
                        Login();
                        break;
                    case '2':
                        validChar = true;
                        NewCustomerMenu();
                        break;
                    case '3':
                        validChar = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk 1 for at logge ind, 2 for at oprette ny bruger\neller 3 for at logge ud.");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);


        }
        public void Login()
        {
            Costumer currentCostumer = null;
            do
            {
                Console.Clear();
                Console.WriteLine("=Login=");
                Console.WriteLine("Skriv dine oplysninger:");
                Console.Write("Username: ");
                string usernameInput = Console.ReadLine();
                Console.Write("Password: ");
                string passwordInput = Console.ReadLine();
                currentCostumer = methods.VerifyLogin(usernameInput, passwordInput);
                if (currentCostumer != null)
                {
                    MainMenu(currentCostumer);
                }
                else
                {
                    Console.WriteLine("De oplysninger du har indtastet findes ikke i systemet.");
                    Console.WriteLine("Tryk på en vilkårlig tast for at prøve igen.");
                    Console.ReadKey(true);
                }
            } while (currentCostumer == null);
        }
        public void MainMenu(Costumer currentCustomer)
        {
            Console.Clear();
            Console.WriteLine($"=Du er logget ind som: {currentCustomer.FullName}=\n");
            Console.WriteLine("Vælg et menupunkt:\n");
            Console.WriteLine("1) Se konti");
            Console.WriteLine("2) Se kort");
            Console.WriteLine("3) Opret ny konto");
            Console.WriteLine("4) Opret nyt kort");
            Console.WriteLine("5) Log ud");
            bool validChar = false;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case '1':
                        validChar = true;
                        SelectAccountMenu(currentCustomer);
                        break;
                    case '2':
                        validChar = true;
                        // CreateAccountMenu(currentCustomer);
                        break;
                    case '3':
                        validChar = true;
                        CreateAccountMenu(currentCustomer, false);
                        break;
                    case '4':
                        validChar = true;
                        CreateCreditCardMenu(currentCustomer);
                        break;
                    case '5':
                        validChar = true;
                        break;
                    default:
                        Console.SetCursorPosition(0, 10);
                        Console.WriteLine("Vælg en menu mellem 1 og 5.");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);
        }

        public void NewCustomerMenu()
        {
            Console.Clear();
            Console.WriteLine("Skriv dit brugernavn");
            string usernameInput = Console.ReadLine();

            string pass1, pass2;
            string passwordInput = "";
            do
            {
                Console.Clear();
                Console.WriteLine("Skriv dit password");
                pass1 = Console.ReadLine();

                Console.WriteLine("Gentag dit password");
                pass2 = Console.ReadLine();

                if (pass1 != pass2)
                {
                    Console.WriteLine("Password matcher ikke.");
                    Console.WriteLine("Tryk på en vilkårlig tast for at prøve igen.");
                    Console.ReadKey();
                }
                else
                {
                    passwordInput = pass1;
                }

            } while (pass1 != pass2);

            Console.Clear();
            Console.WriteLine("Skriv dit fulde navn");
            string fullNameInput = Console.ReadLine();

            Costumer currentCustomer = methods.CreateCostumer(usernameInput, passwordInput, fullNameInput);

            Console.Clear();
            Console.WriteLine("Ny bruger registreret med følgende oplysninger:");
            Console.WriteLine($"Brugernavn: {currentCustomer.Username}");
            Console.WriteLine($"Password: {currentCustomer.Password}");
            Console.WriteLine($"Fulde Navn: {currentCustomer.FullName}\n");

            Console.WriteLine("Vil du oprette en ny konto? (J/N)");
            bool validChar = false;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case 'j':
                        validChar = true;
                        CreateAccountMenu(currentCustomer, true);
                        break;
                    case 'n':
                        validChar = true;
                        MainMenu(currentCustomer);
                        break;
                    default:
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk J eller N");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);
        }

        private void CreateAccountMenu(Costumer currentCustomer, bool newCostumer)
        {
            Console.Clear();
            Console.WriteLine("Vælg hvilken type konto du vil oprette?");
            Console.WriteLine("1) Lønkonto");
            Console.WriteLine("2) Budgetkonto");
            Console.WriteLine("3) Opsparingskonto");
            bool validChar = false;
            int accountChoice = 0;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case '1':
                        validChar = true;
                        accountChoice = 0;
                        break;
                    case '2':
                        validChar = true;
                        accountChoice = 1;
                        break;
                    case '3':
                        validChar = true;
                        accountChoice = 2;
                        break;
                    default:
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk 1, 2 eller 3");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);

            Account newAccount = methods.CreateAccount(currentCustomer, methods.data.accountTypes[accountChoice]);
            Console.Clear();
            Console.WriteLine($"Du har oprettet en {newAccount.AccountType.TypeName}.");
            Console.WriteLine($"Kontonummer: {newAccount.AccountNumber}.");
            Console.WriteLine($"Årlig rente: {newAccount.AccountType.InterestRate}%.");
            Console.WriteLine($"Din saldo: {newAccount.Balance} kr.");
            Console.WriteLine();

            if (newCostumer)
            {
                Console.WriteLine("Vil du oprette et nyt kreditkort? (J/N)");
                validChar = false;
                do
                {
                    Char inputKey = Console.ReadKey(true).KeyChar;
                    switch (inputKey)
                    {
                        case 'j':
                            validChar = true;
                            CreateCreditCardMenu(currentCustomer);
                            break;
                        case 'n':
                            validChar = true;
                            MainMenu(currentCustomer);
                            break;
                        default:
                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine("Tryk J eller N");
                            Console.SetCursorPosition(0, 0);
                            break;
                    }
                } while (!validChar);
            }
            else
            {
                Console.ReadKey();
                MainMenu(currentCustomer);
            }
        }

        public void SelectAccountMenu(Costumer currentCustomer)
        {
            int numberOfAccounts = currentCustomer.accounts.Count();
            Console.Clear();
            for (int i = 0; i < numberOfAccounts; i++)
            {            
            Console.WriteLine(currentCustomer.accounts[i].AccountType.TypeName);
            }

            //Console.WriteLine("Skriv det tal der tilhører den konto du vil vælge");

        }
        public void AccountMenu(Costumer currentCustomer)
        {   //    Console.Clear();
            //    
            //    Console.WriteLine($" {AccountType.TypeName}");
            //    Console.WriteLine($" {AccountNumber}");
            //    Console.WriteLine($" {Saldo}");
            //
            //
            //    Console.WriteLine("1) Konto oversigt ");
            //    Console.WriteLine("2) Overfør peng");  '
            //    Console.WriteLine("3) Gå tilbage til Hoved Menu")
            //    
        }
        public void CreateCreditCardMenu(Costumer currentCustomer)
        {
            Console.Clear();

            int numberOfAccounts = currentCustomer.accounts.Count();

            if (numberOfAccounts == 0)
            {
                Console.WriteLine("Du kan ikke oprette et kort uden at have en konto.");
                
            }
            else if (numberOfAccounts == 1)
            {
                CreditCard newCreditCard = methods.CreateCreditCard(currentCustomer, currentCustomer.accounts[0]);
                Console.Clear();
                Console.WriteLine($"Du har oprettet et nyt kreditkort til kontoen: {currentCustomer.accounts[0].AccountType.TypeName} - {currentCustomer.accounts[0].AccountNumber}.");
                Console.WriteLine($"Kortnummer: {newCreditCard.CardNumber}");
                Console.WriteLine($"Navn: {newCreditCard.FullName}");
                Console.WriteLine($"Udløbsdato: {newCreditCard.ExpDate.Month}/{newCreditCard.ExpDate.Year}");
                Console.WriteLine($"CVC: {newCreditCard.CVC}");
                Console.WriteLine($"PIN-kode: {newCreditCard.PIN}");
                Console.WriteLine($"Årligt gebyr: {newCreditCard.CardFee}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Hvilken af dine konti skal kortet tilknyttes:");
                Console.WriteLine();
                for (int i = 0; i < numberOfAccounts; i++)
                {
                    Console.WriteLine($"{i+1}) {currentCustomer.accounts[i].AccountType.TypeName} - {currentCustomer.accounts[i].AccountNumber}");
                    
                }
                bool validChoice = false;
                bool isAnInt;
                int accountChoice;
                do
                {
                    isAnInt = int.TryParse(Console.ReadLine(), out accountChoice);

                    if (!isAnInt)
                    {
                        validChoice = false;
                        Console.SetCursorPosition(0, numberOfAccounts + 2);
                        Console.WriteLine("Skriv det tal, som tilhører den konto du vil vælge.");
                        Console.SetCursorPosition(0, 0);
                        continue;
                    }
                    else
                    {
                        if (accountChoice < numberOfAccounts)
                        {
                            for (int i = 1; i <= numberOfAccounts; i++)
                            {
                                Account currentAccount;
                                if (i == accountChoice)
                                {
                                    currentAccount = currentCustomer.accounts[i-1];
                                    validChoice = true;

                                    CreditCard newCreditCard = methods.CreateCreditCard(currentCustomer, currentCustomer.accounts[i-1]);
                                    Console.Clear();
                                    Console.WriteLine($"Du har oprettet et nyt kreditkort til kontoen: {currentCustomer.accounts[i-1].AccountType.TypeName} - {currentCustomer.accounts[i-1].AccountNumber}.");
                                    Console.WriteLine($"Kortnummer: {newCreditCard.CardNumber}");
                                    Console.WriteLine($"Navn: {newCreditCard.FullName}");
                                    Console.WriteLine($"Udløbsdato: {newCreditCard.ExpDate.Month}/{newCreditCard.ExpDate.Year}");
                                    Console.WriteLine($"CVC: {newCreditCard.CVC}");
                                    Console.WriteLine($"PIN-kode: {newCreditCard.PIN}");
                                    Console.WriteLine($"Årligt gebyr: {newCreditCard.CardFee}");
                                    Console.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, numberOfAccounts + 2);
                            Console.WriteLine("Den valgte konto eksisterer ikke.");
                            Console.SetCursorPosition(0, 0);
                        }
                    }
                }
                while (!validChoice);
            }
            Console.WriteLine("Tryk på en vilkårlig tast for at vende tilbage til hovedmenuen.");
            Console.ReadKey();
            MainMenu(currentCustomer);
        }
        public void CreditCardMenu()
        {
            //foreach (CreditCard creditCard in creditCards)
            //{
            //    int i++;

            //    Console.WriteLine($" {CreditCard}{1}");
            //    Console.WriteLine($" {CardNumber}");
            //    Console.WriteLine($" {FullName}");
            //    Console.WriteLine($" {ExpDate}");
            //    Console.WriteLine($" {CVC}");
            //    Console.WriteLine($" {CreditCard.PIN}");
            //}

        }
    }
} 
