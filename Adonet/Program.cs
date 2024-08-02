﻿using Adonet.Constants;
using Adonet.Services;

namespace Adonet
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();
                string choiceInput = Console.ReadLine();
                int choice;
                bool isSucceeded = int.TryParse(choiceInput, out choice);
                if (isSucceeded)
                {
                    switch ((Operations)choice)
                    {
                        case Operations.AllCountries:
                            CountryService.GetAllCountries();
                            break;
                        case Operations.AddCountry:
                            CountryService.AddCountry();
                            break;
                        case Operations.UpdateCountry:
                            CountryService.UpdateCountry();
                            break;
                        case Operations.DeleteCountry:
                            CountryService.DeleteCountry();
                            break;
                        case Operations.DetailsOfCountry:
                            CountryService.GetDetailsOfCountry();
                            break;
                        case Operations.Exit:
                            return;
                        default:
                            Messages.InvalidInputMessage("choice");
                            break;
                    }
                }
                else
                {
                    Messages.InvalidInputMessage("choice");
                }
            }
        }
        private static void ShowMenu()
        {
            Console.WriteLine("MENU");
            Console.WriteLine("1 - All countries");
            Console.WriteLine("2 - Add country");
            Console.WriteLine("3 - Update country");
            Console.WriteLine("4 - Delete country");
            Console.WriteLine("5 - Details of country");
            Console.WriteLine("6 - All cities");
            Console.WriteLine("7 - All cities of country");
            Console.WriteLine("8 - Add city");
            Console.WriteLine("9 - Update city");
            Console.WriteLine("10 - Delete city");
            Console.WriteLine("11 - Details of city");
            Console.WriteLine("0 - Exit");
        }
    }
}