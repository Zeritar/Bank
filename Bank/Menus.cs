using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Menus
    {
        Data data = new Data();
        Methods methods = new Methods();
        List<Costumer> costumers = new List<Costumer>();
        public void Welcome()
        {
            methods.CreateFakeData();
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
                        Console.WriteLine("Skriv 1 eller 2");
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
            Console.WriteLine($"Velkommen {currentCustomer.FullName}\n");
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
                        // SeeAccounts();
                        break;
                    case '2':
                        validChar = true;
                        // SeeKarts();
                        break;
                    case '3':
                        validChar = true;
                        CreateAccountMenu(currentCustomer);
                        break;
                    case '4':
                        validChar = true;
                        // CreateNewCreditCard();
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
                        CreateAccountMenu(currentCustomer);
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

        private void CreateAccountMenu(Costumer currentCustomer)
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
                        Console.WriteLine("Skriv 1, 2 eller 3");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);

            methods.CreateAccount(methods.data.accountTypes[accountChoice]);
        }

        public void AccountMenu()
        {
            //List<Account> accounts = new List<Account>();

            //foreach (Account account in accounts)
            //{
            //    int i++;

            //    Console.WriteLine($"{i} {account}");
            //}
        }
        public void CreditCardMenu()
        {

        }
    }
}
