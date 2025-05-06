using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrariiModeleBanking;

namespace Managers
{
    public class ManagerCont
    {
        static int id = 0;
        public enum TipCont
        {
            ContCurent,
            ContEconomii,
            ContInvestitii,
            ContFirma
        }
        public enum Banca
        {
            BTRL,
            BNDR,
            RFSN,
            INGr
        }
        public static void CreeazaCont(List<ContBancar> conturi)
        {
            Console.WriteLine("Introduceti Email");
            string email = Console.ReadLine();

            Console.Write("Introduceti numele si prenumele separate prin spatiu: ");
            string numeComplet = Console.ReadLine();
            string[] nume = numeComplet.Split(' ');
            while(nume.Length < 2)
            {
                Console.WriteLine("Introduceti numele si prenumele corect.");
                numeComplet = Console.ReadLine();
                nume = numeComplet.Split(' ');
           
            }
            Console.WriteLine("Introduceti parola contului:");
            string parola = Console.ReadLine();
            while (parola.Length < 10)
            {
                Console.WriteLine("Parola trebuie sa aiba minim 10 caractere.");
                parola = Console.ReadLine();
            }
            Console.WriteLine("0 - ContCurent \n" +
                              "1 - ContEconomii \n" +
                              "2 - ContInvestitii \n" +
                              "3 - ContFirma ");

            int x = int.Parse(Console.ReadLine());
            while (x<0 || x>3)
            {
                Console.WriteLine("Introduceti o optiune existenta");
                x = int.Parse(Console.ReadLine());
            }
            int optiuneTipCont = Convert.ToInt32(x);
            TipCont tipCont = (TipCont)optiuneTipCont;
            string numeTipCont = Enum.GetName(typeof(TipCont), tipCont); 

            Console.WriteLine("0 - BTRL \n" +
                              "1 - BNDR \n" +
                              "2 - RFSN \n" +
                              "3 - INGr");
            int y = int.Parse(Console.ReadLine());
            while (y < 0 || y > 3)
            {
                Console.WriteLine("Introduceti o optiune existenta");
                y = int.Parse(Console.ReadLine());
            }
            int optiuneBanca = Convert.ToInt32(y);
            Banca banca = (Banca)optiuneBanca;
            string numeBanca = Enum.GetName(typeof(Banca), banca);
            Console.WriteLine(id);
            ContBancar cont = new ContBancar(id, nume[0], nume[1],email,parola, numeTipCont, numeBanca, 0);
            conturi.Add(cont);
            Console.WriteLine($"Cont creat pentru {nume[0]} {nume[1]}.");
            id++;
        }
        public static void AdaugaCardLaCont(List<ContBancar> conturi,int index)
        {
            if (conturi.Count == 0)
            {
                Console.WriteLine("Nu exista conturi create.");
                return;
            }
            
            if (index > 0 && index <= conturi.Count)
            {
                Console.WriteLine("Introduceti parola:");
                string paro = Console.ReadLine();
                int o = 0;
                while (paro != conturi[index].Parola && o<3)
                {
                    Console.WriteLine("Parola incorecta.");
                    paro = Console.ReadLine();
                    o++;
                }
                if(paro == conturi[index].Parola)
                    conturi[index].AdaugaCard();

            }
            else
            {
                Console.WriteLine("Alegere invalida.");
            }
        }
        public static void GestioneazaCont(List<ContBancar> conturi,int x)
        {
            /*if (conturi.Count == 0)
            {
                Console.WriteLine("Nu exista conturi create.");
                return;
            }

            Console.WriteLine("Alegeti contul:");
            for (int i = 0; i < conturi.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {conturi[i].Nume} {conturi[i].Prenume}");
            }

            Console.WriteLine("Introduceti numarul contului: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= conturi.Count)
            {
                conturi[index - 1].UseCard();
            }
            else
            {
                Console.WriteLine("Alegere invalida.");
            }*/
        
            if (conturi.Count == 0)
            {
                Console.WriteLine("Nu exista conturi create.");
                return;
            }
            if (x >= 0)
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
                    conturi[x].UseCard();
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
        }

        
        public static void AfiseazaConturi(List<ContBancar> conturi)
        {
            if (conturi.Count == 0)
            {
                Console.WriteLine("Nu exista conturi create.");
                return;
            }

            Console.WriteLine("\n=== Lista Conturi ===");
            for (int i = 0; i < conturi.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {conturi[i].Nume} {conturi[i].Prenume}, Cont: {conturi[i].TipCont}, IBAN: {conturi[i].IBAN}");
                if (conturi[i].carduri.Count > 0)
                {
                    Console.WriteLine("   Carduri asociate:");
                    foreach (var card in conturi[i].carduri)
                    {
                        string NumarFormat = card.NumarCard.Substring(0, 4) + " " + card.NumarCard.Substring(4, 4) + " " + card.NumarCard.Substring(8, 4) + " " + card.NumarCard.Substring(12, 4);
                        Console.WriteLine($"   - {NumarFormat} (Sold: {card.SoldInitial})");
                    }
                }
                else
                {
                    Console.WriteLine("   Nu exista carduri asociate.");
                }
            }
        }
        public static void SalveazaConturiInFisier(string caleFisierConturi, List<ContBancar> conturi)
        {
            using (StreamWriter sw = new StreamWriter(caleFisierConturi, false))
            {
                //caleFisierConturi = "conturi.txt";
                //Console.WriteLine("Salvare conturi in fisier...");
                Console.WriteLine("Conturi de salvat:");
                foreach (var cont in conturi)
                {
                    Console.WriteLine(cont.ToString());
                }
                //List<string> linii = new List<string>();
                if (conturi.Count == 0)
                {
                    Console.WriteLine("Nu există conturi de salvat.");
                    return;
                }
                foreach (ContBancar cont in conturi)
                {
                    sw.WriteLine(cont.ToString());
                }

            }
        }

        public static void SalveazaCarduriInFisier(string caleFisierCarduri, List<ContBancar> conturi)
        {
            using (StreamWriter writer = new StreamWriter(caleFisierCarduri, false))
            {


                ////caleFisierCarduri = "carduri.txt";
                //Console.WriteLine("Carduri de salvat:");
                //foreach (var cont in conturi)
                //{
                //    foreach (var card in cont.carduri)
                //    {
                //        Console.WriteLine(card.ToString());
                //    }
                //}
                //List<string> linii = new List<string>();
                for (int i = 0; i < conturi.Count; i++)
                {
                    
                    if (conturi[i].carduri.Count > 0)
                    {
                        
                        foreach (var card in conturi[i].carduri)
                        {
                            //Console.WriteLine(card.ToString());
                            writer.WriteLine(card.ToString());
                            //linii.Add(card.ToString());
                        }
                    }
                    else
                    {
                        //Console.WriteLine($"Contul {conturi[i].Nume} {conturi[i].Prenume} nu are carduri asociate.");
                    }
                }

                
            }
        }
        public static List<ContBancar> CitesteConturiDinFisier(string caleFisierConturi, string caleFisierCarduri)
        {


            List<ContBancar> conturi = new List<ContBancar>();
            if (File.Exists(caleFisierConturi))
            {
                
                using (StreamReader reader = new StreamReader(caleFisierConturi))
                {

                    string linie;
                    while ((linie = reader.ReadLine()) != null) 
                    {

                        /*if (string.IsNullOrWhiteSpace(linie)) // Ignorăm liniile goale
                        {
                            continue;
                        }*/

                        string[] date = linie.Split(';');

                        if (date.Length == 7) 
                        {

                            int Rd = int.Parse(date[0]);

                            string nume = date[1];
                            string prenume = date[2];
                            string em = date[3];
                            string par = date[4];
                            string tipCont = date[5];
                            string iban = date[6];

                            ContBancar cont = new ContBancar(Rd, nume, prenume,em,par, tipCont, iban);
                            conturi.Add(cont);
                            id = Rd+1;

                        }
                    }
                }
            }

          
            if (File.Exists(caleFisierCarduri))
            {
                using (StreamReader reader = new StreamReader(caleFisierCarduri))
                {
                    string linie;
                    while ((linie = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(linie)) 
                        {
                            continue;
                        }

                        string[] date = linie.Split(';');
                        if (date.Length == 8) 
                        {
                            int idCont = int.Parse(date[0]);
                            string numarCard = date[1];
                            DateTime dataExpirare = DateTime.ParseExact(date[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            string pin = date[4];
                            int cvv = int.Parse(date[2]);
                            double sold = double.Parse(date[5]);
                            string moneda = date[6];//return $"{Id};{NumarCard};{CVV};{DataExpirare.ToString("dd/MM/yyyy")};{PIN};{SoldInitial};{Moneda}";
                            bool esteActiv = date[7] == "1" ? true : false;
                            Card card = new Card(idCont, cvv, dataExpirare, numarCard, pin, sold, moneda, esteActiv);

                           
                            /*if (cont != null)
                            {
                                cont.carduri.Add(card);
                                Console.WriteLine($"Card adăugat la contul {cont.Nume} {cont.Prenume}: {card}");
                            }
                            else
                            {
                                Console.WriteLine($"Contul cu ID-ul {idCont} nu a fost găsit. Cardul nu a fost adăugat.");
                            }*/
                            for (int i = 0; i < conturi.Count; i++)
                            {
                               
                                if (conturi[i].Id == idCont)
                                {
                                    conturi[i].carduri.Add(card);
                                    //Console.WriteLine($"Card adaugat la contul {conturi[i].Nume} {conturi[i].Prenume}: {card}");
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }

            return conturi;
        }
    }
}
