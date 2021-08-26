using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Views
    {
        // Instans af Methods-klassen
        Methods methods = new Methods();

        // Visning for den valgte konto. Viser kontonavn, kontonummer, saldo og liste over overførsler
        public void AccountView(Account currentAccount)
        {
            Console.Clear();

            // Skriv kontonavn og kontonummer
            Console.WriteLine($"{currentAccount.AccountType.TypeName} - {currentAccount.AccountNumber}");

            // Skriv saldo
            Console.WriteLine($"Saldo: {currentAccount.Balance} kr.\n");

            List<Transaction_View> transactions = methods.GetAllTransactions(currentAccount);
            methods.SortTransactionsByDate(transactions);

            int entries = Math.Min(transactions.Count, 20);

            for (int i = 0; i < entries; i++)
            {
                Transaction_View t_view = transactions[i];
                methods.WriteViewEntry(t_view);
            }

            Console.WriteLine("\n");
        }

        public void CreditCardView(Customer currentCustomer)
        {
            Console.Clear();

            Console.WriteLine("foreach (CreditCard card in currentCustomer.creditCards)");
            Console.WriteLine("{Kortnummer}");
            Console.WriteLine("Tilknyttet konto: {// Find konto med kontonummer i liste over kundes konti}\n\n");
            Console.WriteLine("Tryk på en vilkårlig tast for at vende tilbage til hovedmenu");
            Console.ReadKey();
        }
    }
}
