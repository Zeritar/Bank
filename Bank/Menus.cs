using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Menus
    {
        public Methods methods = new Methods();
        public Views views = new Views();

        // Velkomstskærm
        public void Welcome()
        {

            // Udskriv velkomstskærm
            Console.Clear();
            Console.WriteLine("=Velkommen til Cirkel Bank=");
            Console.WriteLine("Vælg et menupunkt:\n");
            Console.WriteLine("1) Login");
            Console.WriteLine("2) Opret bruger");
            Console.WriteLine("3) Afslut programmet");

            // Tjek hvilket menupunkt brugeren vælger
            bool validChar = false;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case '1':
                        // Gå til loginskærm
                        validChar = true;
                        Login();
                        break;
                    case '2':
                        // Gå til "Ny bruger" skærm
                        validChar = true;
                        NewCustomerMenu();
                        break;
                    case '3':
                        // Afslut programmet
                        validChar = true;
                        Environment.Exit(0);
                        break;
                    default:
                        // Bruger har ikke valgt en gyldig mulighed
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk 1 for at logge ind, 2 for at oprette ny bruger\neller 3 for at logge ud.");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);
        }

        // Loginskærm
        public void Login()
        {
            Customer currentCustomer = null;
            do
            {
                Console.Clear();
                Console.WriteLine("=Login=");
                Console.WriteLine("Skriv dine oplysninger:");

                Console.Write("Brugernavn: ");
                string usernameInput = Console.ReadLine();
                Console.Write("Password: ");
                string passwordInput = methods.GetHiddenPasswordInput();

                // Verificer de indtastede oplysninger fra bruger.
                currentCustomer = methods.VerifyLogin(usernameInput, passwordInput);

                if (currentCustomer != null)
                {
                    // Gå til hovedmenu hvis de indtastede oplysninger matcher en bruger
                    MainMenu(currentCustomer);
                }
                else
                {
                    // Bruger findes ikke eller password er forkert.
                    Console.WriteLine("De oplysninger du har indtastet findes ikke i systemet.");
                    Console.WriteLine("Tryk på en vilkårlig tast for at prøve igen.");
                    Console.ReadKey(true);
                }
            } while (currentCustomer == null);
        }

        // Hovedmenu
        public void MainMenu(Customer currentCustomer)
        {
            // Tjek hvilket menupunkt brugeren vælger
            bool logout = false;
            bool invalidChoice = false;
            do
            {
                // Udskriv hovedmenu
                Console.Clear();
                Console.WriteLine($"=Du er logget ind som: {currentCustomer.FullName}=\n");


                // Tjek om bruger har en konto og et kreditkort
                bool hasAccount = currentCustomer.accounts.Count != 0;
                bool hasCreditCard = currentCustomer.creditCards.Count != 0;
                Methods.CustomerStatus customerStatus;

                if (hasAccount && !hasCreditCard)
                {
                    customerStatus = Methods.CustomerStatus.HasAccount;
                }
                else if (!hasAccount && hasCreditCard)
                {
                    customerStatus = Methods.CustomerStatus.HasCreditCard;
                }
                else if (!hasAccount && !hasCreditCard)
                {
                    customerStatus = Methods.CustomerStatus.HasNeither;
                }
                else
                {
                    customerStatus = Methods.CustomerStatus.HasBoth;

                }

                // Generer hovedmenu baseret på om bruger har konto og kreditkort
                methods.GenerateMainMenu(currentCustomer, customerStatus, invalidChoice);


                // Find den tast bruger trykkede på og lav den til en int hvis vi kan. Default til 0 hvis ikke.
                // Brug metode for at finde ud af hvad brugers input skal gøre

                int inputKey = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out inputKey) ? inputKey : 0;
                string choice = methods.HandleMainMenuChoice(customerStatus, inputKey);

                switch (choice)
                {
                    case "viewAccounts":
                        // Gå til menu for at vælge konti
                        invalidChoice = false;
                        SelectAccountForViewMenu(currentCustomer);
                        break;
                    case "viewCreditCards":
                        // Gå til visning af kreditkort
                        invalidChoice = false;
                        views.CreditCardView(currentCustomer);
                        break;
                    case "createAccount":
                        // Gå til menu for oprettelse af en ny konto
                        invalidChoice = false;
                        CreateAccountMenu(currentCustomer, false);
                        break;
                    case "createCreditCard":
                        // Gå til menu for oprettelse af et nyt kreditkort
                        invalidChoice = false;
                        CreateCreditCardMenu(currentCustomer, false);
                        break;
                    case "createTransaction":
                        // Opret overførsel
                        invalidChoice = false;
                        SelectAccountForTransferMenu(currentCustomer);
                        break;
                    case "logout":
                        // Log ud
                        invalidChoice = false;
                        logout = true;
                        break;
                    default:
                        // Bruger har ikke valgt en gyldig mulighed
                        invalidChoice = true;
                        break;
                }
            } while (!logout);
        }

        // Menu for oprettelse af ny bruger
        public void NewCustomerMenu()
        {
            // Bed om et brugernavn
            Console.Clear();
            Console.WriteLine("Skriv dit brugernavn");
            string usernameInput = Console.ReadLine();

            // TODO: Tjek om bruger har indtastet et brugernavn
            // TODO: Tjek om brugernavn allerede findes

            string pass1, pass2;
            string passwordInput = "";
            do
            {
                // Bed om password 2 gange og tjek om bruger har indtastet det samme begge gange
                Console.Clear();
                Console.WriteLine("Skriv dit password");
                pass1 = methods.GetHiddenPasswordInput();
                Console.Write("\n");
                Console.WriteLine("Gentag dit password");
                pass2 = methods.GetHiddenPasswordInput();
                Console.Write("\n");
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

            // Bed om brugers fulde navn
            Console.Clear();
            Console.WriteLine("Skriv dit fulde navn");
            string fullNameInput = Console.ReadLine();

            // Opret den nye bruger og tilføj til listen over brugere
            Customer currentCustomer = methods.CreateCustomer(usernameInput, passwordInput, fullNameInput);

            Console.Clear();
            Console.WriteLine("Ny bruger registreret med følgende oplysninger:");
            Console.WriteLine($"Brugernavn: {currentCustomer.Username}");
            Console.WriteLine($"Password: {currentCustomer.Password}");
            Console.WriteLine($"Fulde Navn: {currentCustomer.FullName}\n");

            // Spørg bruger om de vil oprette en ny konto
            Console.WriteLine("Vil du oprette en ny konto? (J/N)");
            bool validChar = false;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case 'j':
                        // Hvis ja, gå til Opret Konto menu
                        validChar = true;
                        CreateAccountMenu(currentCustomer, true);
                        break;
                    case 'n':
                        // Hvis nej, gå til hovedmenu
                        validChar = true;
                        MainMenu(currentCustomer);
                        break;
                    default:
                        // Bruger har ikke valgt en gyldig mulighed
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk J eller N");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);
        }

        // Menu for oprettelse af ny konto
        private void CreateAccountMenu(Customer currentCustomer, bool newCustomer)  // newCustomer sørger for at vi ikke spørger allerede
                                                                                    // eksisterende kunder om de vil oprette et kreditkort
        {
            // Udskriv menu for oprettelse af konto
            Console.Clear();
            Console.WriteLine("Vælg hvilken type konto du vil oprette:");
            Console.WriteLine("1) Lønkonto");
            Console.WriteLine("2) Budgetkonto");
            Console.WriteLine("3) Opsparingskonto");

            // Læs input fra bruger og sæt kontovalg til den tilsvarende værdi
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
                        // Bruger har ikke valgt en gyldig kontotype
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk 1, 2 eller 3");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);

            // Opret en ny konto af den valgte kontotype og tilføj den til brugers
            // liste over konti
            Account newAccount = methods.CreateAccount(currentCustomer, methods.data.accountTypes[accountChoice]);
            Console.Clear();
            Console.WriteLine($"Du har oprettet en {newAccount.AccountType.TypeName}.");
            Console.WriteLine($"Kontonummer: {newAccount.AccountNumber}.");
            Console.WriteLine($"Årlig rente: {newAccount.AccountType.InterestRate}%.");
            Console.WriteLine($"Din saldo: {newAccount.Balance} kr.");
            Console.WriteLine();

            // Hvis bruger lige er oprettet, spørg om de vil oprette et kreditkort
            if (newCustomer)
            {
                Console.WriteLine("Vil du oprette et nyt kreditkort? (J/N)");
                validChar = false;
                do
                {
                    Char inputKey = Console.ReadKey(true).KeyChar;
                    switch (inputKey)
                    {
                        case 'j':
                            // Gå til menu for oprettelse af kreditkort
                            validChar = true;
                            CreateCreditCardMenu(currentCustomer, true);
                            break;
                        case 'n':
                            // Gå til hovedmenu
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
            // Hvis bruger ikke lige er oprettet, gå til hovedmenu
            // Vi har allerede en instans af hovedmenuen i gang. Afslut metoden uden at åbne en ny hovedmenu
            else
            {
                Console.WriteLine("Tryk på en vilkårlig tast for at vende tilbage til hovedmenuen.");
                Console.ReadKey();
            }
        }

        // Menu for at vælge en konto
        public void SelectAccountForViewMenu(Customer currentCustomer)
        {
            // Tæl hvor mange konti bruger har
            int numberOfAccounts = currentCustomer.accounts.Count();
            Console.Clear();

            // Hvis bruger ikke har en konto, spørg om de vil oprette en konto
            if (numberOfAccounts == 0)
            {
                Console.WriteLine("Du har ikke en konto. Vil du oprette en konto nu? (J/N)");
                bool validChar = false;
                do
                {
                    Char inputKey = Console.ReadKey(true).KeyChar;
                    switch (inputKey)
                    {
                        case 'j':
                            // Hvis ja, gå til Opret Konto menu
                            validChar = true;
                            CreateAccountMenu(currentCustomer, true);
                            break;
                        case 'n':
                            // Hvis nej, gå til hovedmenu
                            validChar = true;
                            MainMenu(currentCustomer);
                            break;
                        default:
                            // Bruger har ikke valgt en gyldig mulighed
                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine("Tryk J eller N");
                            Console.SetCursorPosition(0, 0);
                            break;
                    }
                } while (!validChar);

            }
            // Hvis bruger kun har én konto, gå til kontovisning
            else if (numberOfAccounts == 1)
            {
                views.AccountView(currentCustomer.accounts[0]);
            }
            // Hvis bruger har 2 eller flere konti, vælg hvilken konto de vil se
            else
            {
                Console.WriteLine("Hvilken konto vil du se?");
                Console.WriteLine();
                for (int i = 0; i < numberOfAccounts; i++)
                {
                    Console.WriteLine($"{i + 1}) {currentCustomer.accounts[i].AccountType.TypeName} - {currentCustomer.accounts[i].AccountNumber}: {currentCustomer.accounts[i].Balance} kr.");

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
                        Console.SetCursorPosition(0, numberOfAccounts + 3);
                        Console.WriteLine("Skriv tallet på den konto du vil vælge.");
                        Console.SetCursorPosition(0, numberOfAccounts + 2);
                        continue;
                    }
                    else
                    {
                        if (accountChoice <= numberOfAccounts)
                        {
                            for (int i = 1; i <= numberOfAccounts; i++)
                            {
                                Account currentAccount;
                                if (i == accountChoice)
                                {
                                    currentAccount = currentCustomer.accounts[i - 1];
                                    validChoice = true;

                                    // Gå til kontovisning
                                    views.AccountView(currentAccount);
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, numberOfAccounts + 3);
                            Console.WriteLine("Den valgte konto eksisterer ikke.");
                            Console.SetCursorPosition(0, numberOfAccounts + 2);
                        }
                    }
                }
                while (!validChoice);
            }
            Console.WriteLine("Tryk på en vilkårlig tast for at vende tilbage til hovedmenuen.");
            Console.ReadKey();
        }

        // Menu for at vælge en konto til overførsl
        public void SelectAccountForTransferMenu(Customer currentCustomer)
        {
            // Tæl hvor mange konti bruger har
            int numberOfAccounts = currentCustomer.accounts.Count();
            Console.Clear();

            // Hvis bruger ikke har en konto, spørg om de vil oprette en konto
            if (numberOfAccounts == 0)
            {
                Console.WriteLine("Du har ikke en konto. Vil du oprette en konto nu? (J/N)");
                bool validChar = false;
                do
                {
                    Char inputKey = Console.ReadKey(true).KeyChar;
                    switch (inputKey)
                    {
                        case 'j':
                            // Hvis ja, gå til Opret Konto menu
                            validChar = true;
                            CreateAccountMenu(currentCustomer, true);
                            break;
                        case 'n':
                            // Hvis nej, gå til hovedmenu
                            validChar = true;
                            MainMenu(currentCustomer);
                            break;
                        default:
                            // Bruger har ikke valgt en gyldig mulighed
                            Console.SetCursorPosition(0, 8);
                            Console.WriteLine("Tryk J eller N");
                            Console.SetCursorPosition(0, 0);
                            break;
                    }
                } while (!validChar);

            }
            // Hvis bruger kun har én konto, gå til kontovisning
            else if (numberOfAccounts == 1)
            {
                CreateTransferMenu(currentCustomer.accounts[0]);
            }
            // Hvis bruger har 2 eller flere konti, vælg hvilken konto de vil se
            else
            {
                Console.WriteLine("Hvilken konto vil du se?");
                Console.WriteLine();
                for (int i = 0; i < numberOfAccounts; i++)
                {
                    Console.WriteLine($"{i + 1}) {currentCustomer.accounts[i].AccountType.TypeName} - {currentCustomer.accounts[i].AccountNumber}: {currentCustomer.accounts[i].Balance} kr.");

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
                        Console.SetCursorPosition(0, numberOfAccounts + 3);
                        Console.WriteLine("Skriv tallet på den konto du vil vælge.");
                        Console.SetCursorPosition(0, numberOfAccounts + 2);
                        continue;
                    }
                    else
                    {
                        if (accountChoice <= numberOfAccounts)
                        {
                            for (int i = 1; i <= numberOfAccounts; i++)
                            {
                                Account currentAccount;
                                if (i == accountChoice)
                                {
                                    currentAccount = currentCustomer.accounts[i - 1];
                                    validChoice = true;

                                    CreateTransferMenu(currentCustomer.accounts[i - 1]);
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, numberOfAccounts + 3);
                            Console.WriteLine("Den valgte konto eksisterer ikke.");
                            Console.SetCursorPosition(0, numberOfAccounts + 2);
                        }
                    }
                }
                while (!validChoice);
            }
            Console.WriteLine("Tryk på en vilkårlig tast for at vende tilbage til hovedmenuen.");
            Console.ReadKey();
        }

        public void CreateTransferMenu(Account currentAccount)
        {
            Console.Clear();
            Console.WriteLine("Hvad vil du foretage dig?");
            Console.WriteLine("1) Betal");
            Console.WriteLine("2) Indsæt");
            Console.WriteLine("3) Afbryd");

            // Læs input fra bruger og start oprettelse af valgt overførselstype
            bool validChar = false;
            bool payment = false;
            bool cancel = false;
            do
            {
                Char inputKey = Console.ReadKey(true).KeyChar;
                switch (inputKey)
                {
                    case '1':
                        validChar = true;
                        payment = true;
                        break;
                    case '2':
                        validChar = true;
                        break;
                    case '3':
                        validChar = true;
                        cancel = true;
                        break;
                    default:
                        // Bruger har ikke valgt en gyldig tast
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Tryk 1, 2 eller 3");
                        Console.SetCursorPosition(0, 0);
                        break;
                }
            } while (!validChar);

            if (cancel)
            {
                // Afbryd og gå til hovedmenu
            }
            else
            {
                Console.Clear();
                if (payment)
                {
                    Console.WriteLine("=Ny betaling=");
                    Console.Write("Modtagers kontonummer: ");
                    string recipient = Console.ReadLine();
                    double amount;
                    bool validAmount;
                    int consolePosX = Console.GetCursorPosition().Left;
                    int consolePosY = Console.GetCursorPosition().Top;
                    do
                    {
                        // Jank metode for at bede om beløb
                        // Overskriver den samme linje ved fejl
                        Console.SetCursorPosition(consolePosX, consolePosY);
                        Console.Write("                                                 ");
                        Console.SetCursorPosition(consolePosX, consolePosY);
                        Console.Write("Beløb: ");

                        // Tjek om bruger har skrevet et gyldigt beløb
                        validAmount = double.TryParse(Console.ReadLine(), out amount);
                        if (!validAmount)
                        {
                            Console.WriteLine("Du skal skrive et gyldigt beløb.");
                        }

                    } while (!validAmount);
                    Console.WriteLine("Skriv tekst som skal stå på kontoudskrift: ");
                    string description = Console.ReadLine();

                    // Opret ny betaling med de indtastede oplysninger, med gebyr.
                    methods.CreateTransaction_Out(currentAccount, description, amount, DateTime.Now, true, recipient);

                    Console.Clear();
                    Console.WriteLine("Betalingen er oprettet og kan ses på kontoudskriften.\n");
                }
                else
                {
                    Console.WriteLine("=Ny betaling=");
                    Console.WriteLine("Afsenders kontonummer: ");
                    string sender = Console.ReadLine();
                    double amount;
                    bool validAmount;
                    int consolePosX = Console.GetCursorPosition().Left;
                    int consolePosY = Console.GetCursorPosition().Top;
                    do
                    {
                        // Jank metode for at bede om beløb
                        // Overskriver den samme linje ved fejl
                        Console.SetCursorPosition(consolePosX, consolePosY);
                        Console.Write("                                                 ");
                        Console.SetCursorPosition(consolePosX, consolePosY);
                        Console.Write("Beløb: ");

                        // Tjek om bruger har skrevet et gyldigt beløb
                        validAmount = double.TryParse(Console.ReadLine(), out amount);
                        if (!validAmount)
                        {
                            Console.WriteLine("Du skal skrive et gyldigt beløb.");
                        }

                    } while (!validAmount);
                    Console.WriteLine("Skriv tekst som skal stå på kontoudskrift: ");
                    string description = Console.ReadLine();

                    // Opret ny indbetaling med de indtastede oplysninger, uden gebyr.
                    methods.CreateTransaction_In(currentAccount, description, amount, DateTime.Now, false, sender);

                    Console.Clear();
                    Console.WriteLine("Indbetalingen er oprettet og kan ses på kontoudskriften.\n");
                }
            }
        }

        // Kontomenu
        public void AccountMenu(Customer currentCustomer)
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

        // Menu for at oprette et kreditkort
        public void CreateCreditCardMenu(Customer currentCustomer, bool newCustomer) // newCustomer sørger for at vi ikke opretter unødvendige instanser af hovedmenu
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
                Console.WriteLine($"Du har oprettet et nyt kreditkort til kontoen: {currentCustomer.accounts[0].AccountType.TypeName} - {currentCustomer.accounts[0].AccountNumber}.\n");
                Console.WriteLine($"Kortnummer:    {newCreditCard.CardNumber}");
                Console.WriteLine($"Navn:          {newCreditCard.FullName}");
                Console.WriteLine($"Udløbsdato:    {newCreditCard.ExpDate.Month.ToString().PadLeft(2,'0')}/{newCreditCard.ExpDate.Year.ToString()[^2..^0]}");
                Console.WriteLine($"CVC:           {newCreditCard.CVC}");
                Console.WriteLine($"PIN-kode:      {newCreditCard.PIN}");
                Console.WriteLine($"Årligt gebyr:  {newCreditCard.CardFee}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Hvilken konto skal kortet tilknyttes:");
                Console.WriteLine();
                for (int i = 0; i < numberOfAccounts; i++)
                {
                    Console.WriteLine($"{i + 1}) {currentCustomer.accounts[i].AccountType.TypeName} - {currentCustomer.accounts[i].AccountNumber}");

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
                        Console.SetCursorPosition(0, numberOfAccounts + 3);
                        Console.WriteLine("Skriv det tal, som tilhører den konto du vil vælge.");
                        Console.SetCursorPosition(0, numberOfAccounts + 2);
                        continue;
                    }
                    else
                    {
                        if (accountChoice <= numberOfAccounts)
                        {
                            for (int i = 1; i <= numberOfAccounts; i++)
                            {
                                Account currentAccount;
                                if (i == accountChoice)
                                {
                                    currentAccount = currentCustomer.accounts[i - 1];
                                    validChoice = true;

                                    CreditCard newCreditCard = methods.CreateCreditCard(currentCustomer, currentCustomer.accounts[i - 1]);
                                    Console.Clear();
                                    Console.WriteLine($"Du har oprettet et nyt kreditkort til kontoen: {currentCustomer.accounts[i - 1].AccountType.TypeName} - {currentCustomer.accounts[i - 1].AccountNumber}.\n");
                                    Console.WriteLine($"Kortnummer:    {newCreditCard.CardNumber}");
                                    Console.WriteLine($"Navn:          {newCreditCard.FullName}");
                                    Console.WriteLine($"Udløbsdato:    {newCreditCard.ExpDate.Month.ToString().PadLeft(2, '0')}/{newCreditCard.ExpDate.Year.ToString()[^2..^0]}");
                                    Console.WriteLine($"CVC:           {newCreditCard.CVC}");
                                    Console.WriteLine($"PIN-kode:      {newCreditCard.PIN}");
                                    Console.WriteLine($"Årligt gebyr:  {newCreditCard.CardFee}");
                                    Console.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(0, numberOfAccounts + 3);
                            Console.WriteLine("Den valgte konto eksisterer ikke.");
                            Console.SetCursorPosition(0, numberOfAccounts + 2);
                        }
                    }
                }
                while (!validChoice);
            }
            Console.WriteLine("Tryk på en vilkårlig tast for at vende tilbage til hovedmenuen.");
            Console.ReadKey();
            if (newCustomer)
                MainMenu(currentCustomer);
            // Hvis vi ikke er en ny kunde, så har vi allerede en instans af hovedmenuen i gang. Afslut metoden uden at åbne en ny hovedmenu
        }
    }
}
