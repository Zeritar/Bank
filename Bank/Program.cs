using System;
using System.Collections.Generic;

namespace Bank
{
    class Program
    {
        string[] kontoTyper = new string[] { "Løn", "Budget", "Opsparing", "Pension" };
        static void Main(string[] args)
        {
            Menus menus = new Menus();

            while (true)
            {
                menus.Welcome();
            }
        }
    }
}