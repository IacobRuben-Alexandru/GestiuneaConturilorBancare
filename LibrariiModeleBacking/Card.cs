using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace LibrariiModeleBanking
{
    public class Card
    {
        public enum Valuta
        {
            RON,
            EUR,
            USD
        }
        public const Valuta Standard = Valuta.RON;

        public bool EsteActiv { get; set; } = true;
        public int Id { get; set; }
        public int CVV { get; set; }
        public DateTime DataExpirare { get; set; }
        public string NumarCard { get; set; }
        public string PIN { get; set; }
        public double SoldInitial { get; set; }
        public string Moneda { get; set; }
        public Card() { }
        public Card(int id,int CVV, DateTime DataExpirare, string NumarCard, string Pin, double sold, string moneda, bool esteActiv)
        {
            this.Id = id;
            this.CVV = CVV;
            this.DataExpirare = DataExpirare;
            this.NumarCard = NumarCard;
            this.PIN = Pin;
            this.SoldInitial = sold;
            this.Moneda = moneda;
            this.EsteActiv = esteActiv;
        }
        public override string ToString()
        {
            string NumarFormat = NumarCard.Substring(0, 4) + " " + NumarCard.Substring(4, 4) + " " + NumarCard.Substring(8, 4) + " " + NumarCard.Substring(12, 4);
            return $"{Id};{NumarCard};{CVV};{DataExpirare.ToString("dd/MM/yyyy")};{PIN};{SoldInitial};{Moneda};{(EsteActiv ? 1 : 0)}";

        }
        public Card CardNou(int id)
        {
            Console.WriteLine("Alegeti moneda: ");
            Console.WriteLine("0 - RON \n" +
            "1 - EUR \n" +
            "2 - USD \n");
            int alegere = Convert.ToInt32(Console.ReadLine());
            Valuta mod = (Valuta)alegere;

            //Generators generators = new Generators();
            Console.WriteLine("Introduceti PIN-ul");
            string pin = Console.ReadLine();
            while (pin.Length != 4)
            {
                Console.WriteLine("PIN-ul trebuie sa aiba 4 cifre");
                Console.WriteLine("Introduceti PIN-ul");
                pin = Console.ReadLine();
            }
            int r = (new Random()).Next(100, 1000);
            string Digits = CreateUnique16DigitString();


            Console.WriteLine("Introduceti suma pe care doriti sa o adaugati in cont:");
            Console.WriteLine("Suma minima este 100;");
            double sum = Convert.ToDouble(Console.ReadLine());
            while (sum < 100)
            {
                Console.WriteLine("Suma minima este 100;");
                Console.WriteLine("Introduceti suma pe care doriti sa o adaugati in cont:");
                sum = Convert.ToDouble(Console.ReadLine());
            }
            Card card1 = new Card(id,r, DateTime.Today.AddYears(4), Digits, pin, sum, mod.ToString(),true);
            return card1;
        }
        private static HashSet<string> Results = new HashSet<string>();

        private static Random RNG = new Random();
        public string Create16DigitString()
        {
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10).ToString());
            }
            return builder.ToString();
        }
        public string CreateUnique16DigitString()
        {
            var result = Create16DigitString();
            while (!Results.Add(result))
            {
                result = Create16DigitString();
            }

            return result;
        }

    }
}
