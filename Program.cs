using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H4BankVault
{
    class Program
    {
        static void Main(string[] args)
        {
            Services services = new Services();
            Menu(services);
        }

        private static void Menu(Services service)
        {
            Console.WriteLine("Velkommen til Bankboksen");
            string menuChoice;
            Account myAccount = null;
            do
            {
                Console.Clear();
                Console.WriteLine("Menu:\n1. Opret ny bruger\n2. Login\n3. Luk programmet");
                menuChoice = Console.ReadLine();
                switch (menuChoice)
                {
                    case "1":
                        string note = "";
                        Console.WriteLine("Indtast en ønsket kode");
                        string password = Console.ReadLine();
                        Console.WriteLine("Ønsker du at skrive en note til din konto?\n1. Ja");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.WriteLine("Skriv note");
                                note = Console.ReadLine();
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine($"Dit nye konto nummer er:" + service.CreateAccount(password, note).AccountId);
                        break;
                    case "2":
                        try
                        {
                            Console.WriteLine("Indtast dit konto nummer");
                            int accountId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Indtast dit kodeord");
                            myAccount = service.GetAccount(accountId, Console.ReadLine());
                            Console.Clear();
                            while (myAccount != null)
                            {
                                Console.WriteLine("Her er dine mulighedder:\n1. Skriv en ny note\n2. Se din note\n3. Se din saldo\n4. Log ud\n5. Slet konto");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        Console.WriteLine("Skriv note");
                                        myAccount = service.AccountNote(myAccount, Console.ReadLine());
                                        break;
                                    case "2":
                                        Console.WriteLine(service.GetNote(myAccount));
                                        break;
                                    case "3":
                                        Console.WriteLine(myAccount.Banlance);
                                        break;
                                    case "4":
                                        myAccount = null;
                                        break;
                                    case "5":
                                        service.DeleteAccount(myAccount);
                                        myAccount = null;
                                        break;
                                    default:
                                        break;
                                }
                                service.SaveAccounts();
                            }
                        }
                        catch (Exception m)
                        {
                            Console.WriteLine(m.Message);
                            Thread.Sleep(3000);
                        }
                        break;
                    default:
                        break;
                }
                service.SaveAccounts();
            } while (menuChoice != "3");
        }
    }
}
