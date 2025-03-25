using System;
using System.Collections.Generic;
using LibrariiModeleBanking;

namespace Managers
{
    public class ManagerUtilizatori
    {
        static public void EliminaUtilizator(List<ContBancar> conturi, string cont)
        {
            int x = -1;
            if (conturi.Count == 0)
            {
                Console.WriteLine("Nu exista conturi create.");
                return;
            }

            for (int i = 0; i < conturi.Count; i++)
            {
                //Console.WriteLine($"{i + 1}. {conturi[i].Nume} {conturi[i].Prenume}");
                if (conturi[i].Email == cont)
                { x = i;}
            }

            if (x>=0)
            {
                Console.WriteLine("Introduceti parola:");
                string parola = Console.ReadLine();
                int incercari = 0;

                while (parola != conturi[x].Parola && incercari < 3)
                {
                    Console.WriteLine("Parola incorecta. Mai incercati o data:");
                    parola = Console.ReadLine();
                    incercari++;
                }

                if (parola == conturi[x].Parola)
                {
                    // Ștergem cardurile asociate contului
                    conturi[x].carduri.Clear();

                    // Ștergem contul din listă
                    conturi.RemoveAt(x);

                    Console.WriteLine("Contul si cardurile asociate au fost eliminate cu succes.");
                }
                else
                {
                    Console.WriteLine("Numar maxim de incercari depasit. Contul nu a fost eliminat.");
                }
            }
            else
            {
                Console.WriteLine("Alegere invalida.");
            }
        }
        public static int AutentificareAdmin(List<ContBancar> conturi)
        {
            int x = -1;
            Console.WriteLine("Introduceti emailul:");
            string CONT = Console.ReadLine();
            for (int i = 0; i < conturi.Count; i++)
            {
                //Console.WriteLine($"{i + 1}. {conturi[i].Nume} {conturi[i].Prenume}");
                if (conturi[i].Email == CONT)
                { x = i; }
            }
            if (x == 0)
            {
                Console.WriteLine("Introduceti parola:");
                string parola = Console.ReadLine();
                int incercari = 0;

                while (parola != conturi[x].Parola && incercari < 3)
                {
                    Console.WriteLine("Parola incorecta. Mai incercati o data:");
                    parola = Console.ReadLine();
                    incercari++;
                }

                if (parola == conturi[x].Parola)
                {
                    return 1;
                }
                else
                {
                    Console.WriteLine("Ati depasit numaru maxim de incercari!");
                }
            }
            else
            {
                Console.WriteLine("Alegere invalida.");
            }
            return 0;
        }
        public static int AutentificareUtilizator(List<ContBancar> conturi)
        {
            int x = -1;
            Console.WriteLine("Introduceti emailul:");
            string CONT = Console.ReadLine();
            for (int i = 1; i < conturi.Count; i++)
            {
                //Console.WriteLine($"{i + 1}. {conturi[i].Nume} {conturi[i].Prenume}");
                if (conturi[i].Email == CONT)
                { x = i; }
            }
            if (x > 0)
            {
                Console.WriteLine("Introduceti parola:");
                string parola = Console.ReadLine();
                int incercari = 0;

                while (parola != conturi[x].Parola && incercari < 3)
                {
                    Console.WriteLine("Parola incorecta. Mai incercati o data:");
                    parola = Console.ReadLine();
                    incercari++;
                }

                if (parola == conturi[x].Parola)
                {
                    return x;
                }
                else
                {
                    Console.WriteLine("Ati depasit numaru maxim de incercari!");
                }
            }
            else
            {
                Console.WriteLine("Alegere invalida.");
            }
            return -1;
        }
    }
}