using System;
using System.Collections.Generic;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instans af Menus-klassen
            Menus menus = new Menus();

            // Udfyld fake data
            menus.methods.GetAccountTypes();
            menus.methods.CreateFakeData();

            // menus.CreateTransferMenu(testAccount);

            while (true)
            {
                // Gå til velkomst-skærmen
                menus.Welcome();
            }
        }
    }
}