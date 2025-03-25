using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using Managers;
using LibrariiModeleBanking;
using System.ComponentModel.Design;
namespace GestiuneaConturilorBancare
{
    class Program
    {


        static void Main(string[] args)
        {
            List<ContBancar> conturi = ManagerCont.CitesteConturiDinFisier("../../conturi.txt", "../../carduri.txt");
            
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Meniu Principal ===");
                Console.WriteLine("1. Creeaza un cont nou");
                Console.WriteLine("2. Autentificare Utilizator");
                Console.WriteLine("3. Autentificare Admin");
                Console.WriteLine("4. Salveaza datele in fisier");
                Console.WriteLine("5. Iesire");
           
                Console.Write("Alegeti o optiune: ");
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1": 
                        ManagerCont.CreeazaCont(conturi);
                        break;

                    case "2":
                        int Q = ManagerUtilizatori.AutentificareUtilizator(conturi);
                        if (Q == -1)
                        {
                            Console.WriteLine("Autentificare esuata");
                        }
                        else
                        {
                            while (Q != 0)
                            {
                                Console.WriteLine("1. Adauga un card la un cont existent");
                                Console.WriteLine("2. Gestioneaza un cont (depunere/retragere/transfer)");
                                Console.WriteLine("3. Revenire Meniu;");
                                string opt = Console.ReadLine();
                                switch (opt)
                                {
                                    case "1":
                                        ManagerCont.AdaugaCardLaCont(conturi, Q);
                                        break;
                                    case "2":
                                        ManagerCont.GestioneazaCont(conturi, Q);
                                        break;
                                    case "3":
                                        goto Men;
                                }
                            }
                        }
                        Men:
                        break;
                    case "3":
                        int a = ManagerUtilizatori.AutentificareAdmin(conturi);
                        if (a == 0)
                        {
                            Console.WriteLine("Autentificare esuata");
                        }
                        else
                        {
                            while (a != 0)
                            {
                                Console.WriteLine("1. Afiseaza Conturi");
                                Console.WriteLine("2. Sterge cont");
                                Console.WriteLine("3. Revenire Meniu;");
                                string opt = Console.ReadLine();
                                switch (opt)
                                {
                                    case "1":
                                        ManagerCont.AfiseazaConturi(conturi);
                                        break;
                                    case "2":
                                        Console.WriteLine("Ce cont dorest sa stergi, introdu emailul?:");
                                        string c = Console.ReadLine();
                                        ManagerUtilizatori.EliminaUtilizator(conturi, c);
                                        break;
                                    case "3":
                                        goto Meniu;
                                }
                            }
                        }
                        Meniu:
                        break;

                    case "4":
                        ManagerCont.SalveazaCarduriInFisier("../../carduri.txt", conturi);
                        ManagerCont.SalveazaConturiInFisier("../../conturi.txt", conturi);
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Aplicatia se inchide...");
                        break;     
                    default:
                        Console.WriteLine("Optiune invalida. Incercati din nou.");
                        break;
                }
            }
        }
        

    }
    }
