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

            while (true)
            {
                // Gå til velkomst-skærmen
                menus.Welcome();
            }
        }
    }
}