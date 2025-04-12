//using IbanNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//using static GestiuneaConturilorBancare.Program;

namespace LibrariiModeleBanking
{
     
    public class ContBancar
    {
        public List<Card> carduri { get; set; } = new List<Card>();
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; } 
        public string TipCont { get; set; }
        public string IBAN { get; set; }
        public string Parola { get; set; }
        public string Banca { get; set; }
        public string Email { get; set; }   

        public ContBancar()
        { }
        public ContBancar(int id, string nume, string prenume,string email, string parola,string tipcont,string iban)
        {
            this.Id = id;
            this.Nume = nume;
            this.Prenume = prenume;
            this.Email = email;
            this.Parola = parola;
            this.TipCont = tipcont;
            this.IBAN = iban;
        }
        public ContBancar(int id,string nume,string prenume,string email,string parola,string tipcont,string banca,int ib)
        {
            this.Id = id;
            this.Nume = nume;
            this.Prenume = prenume;
            this.Email = email;
            this.Parola = parola;
            this.TipCont = tipcont;
            this.Banca = banca;
            
            IBAN = GenereazaIban(Banca);
        }

        public override string ToString()
        {
            Console.WriteLine($"ID: {Id}, Nume: {Nume}, Prenume: {Prenume},Email: {Email}, Parola: {Parola} TipCont: {TipCont}, IBAN: {IBAN}");
            return $"{Id};{Nume};{Prenume};{Email};{Parola};{TipCont};{IBAN}";
        }
        
        public void AdaugaCard()
        {
            Card card = new Card();
            card = card.CardNou(Id);
            carduri.Add(card);
        }
        public void UseCard()
        {
            if (carduri.Count == 0)
            {
                Console.WriteLine("Nu exista carduri asociate acestui cont.");
                return;
            }

            Console.WriteLine("Alegeti cardul:");
            for (int i = 0; i < carduri.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {carduri[i].NumarCard} in {carduri[i].Moneda}");
            }

            Console.WriteLine("Introduceti numarul cardului: ");
           
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= carduri.Count)
            {
                Console.WriteLine("Introduceti pin:");
                string pin = Console.ReadLine();
                if (pin == carduri[index-1].PIN)
                {
                    Console.WriteLine("Autentificare reusita.");
                    Card cardSelectat = carduri[index - 1];
                    Console.WriteLine("Alegeti operatia:");
                    Console.WriteLine("1. Depunere");
                    Console.WriteLine("2. Retragere");
                    Console.WriteLine("3. Transfer");
                    Console.Write("Optiune: ");
                    string optiune = Console.ReadLine();

                    switch (optiune)
                    {
                        case "1":
                            Console.WriteLine("Introduceti suma: ");
                            if (double.TryParse(Console.ReadLine(), out double sumaDepunere))
                            {
                                OperatiiBancare.Depunere(sumaDepunere, cardSelectat);
                            }
                            else
                            {
                                Console.WriteLine("Suma invalida.");
                            }
                            break;

                        case "2":
                            Console.Write("Introduceti suma: ");
                            Console.WriteLine("Suma din cont:");
                            Console.WriteLine(cardSelectat.SoldInitial);
                            if (double.TryParse(Console.ReadLine(), out double sumaRetragere))
                            {
                                OperatiiBancare.Retragere(sumaRetragere, cardSelectat);
                            }
                            else
                            {
                                Console.WriteLine("Suma invalida.");
                            }
                            break;

                        case "3":
                            Console.Write("Introduceti suma: ");
                            Console.WriteLine("Suma din cont:");
                            Console.WriteLine(cardSelectat.SoldInitial);
                            if (double.TryParse(Console.ReadLine(), out double sumaTransfer))
                            {
                                Console.WriteLine("Alegeti cardul destinatie:");
                                for (int i = 0; i < carduri.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {carduri[i].NumarCard}");
                                }
                                if (int.TryParse(Console.ReadLine(), out int indexDestinatie) && indexDestinatie > 0 && indexDestinatie <= carduri.Count)
                                {
                                    Card cardDestinatie = carduri[indexDestinatie - 1];
                                    OperatiiBancare.Transfer(sumaTransfer, cardSelectat, cardDestinatie);
                                }
                                else
                                {
                                    Console.WriteLine("Alegere invalida.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Suma invalida.");
                            }
                            break;

                        default:
                            Console.WriteLine("Optiune invalida.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Alegere invalida.");
                }
            }
                else
                {
                    Console.WriteLine("Pin incorect.");
                    return;
                }
                
        }
        public static string GenereazaIban(string s)
        {
            string countryCode = "RO";
            string bankCode = s; 
            string branchCode = "1B31";
            string accountNumber = GenerateRandomDigits(12); 

            string ibanWithoutChecksum = countryCode + "00" + bankCode + branchCode + accountNumber;
            string checksum = CalculateIbanChecksum(ibanWithoutChecksum);
            string validIban = countryCode + checksum + bankCode + branchCode + accountNumber;

            return FormatIban(validIban);
        }

        
        private static string GenerateRandomDigits(int length)
        {
            Random rand = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => (char)('0' + rand.Next(0, 10))).ToArray());
        }

        
        private static string CalculateIbanChecksum(string ibanWithoutChecksum)
        {
            string rearranged = ibanWithoutChecksum.Substring(4) + ibanWithoutChecksum.Substring(0, 4);
            string numericIban = ConvertToNumericString(rearranged);

            BigInteger ibanNumber = BigInteger.Parse(numericIban);
            int checksum = (98 - (int)(ibanNumber % 97));

            return checksum.ToString("D2");
        }

        
        private static string ConvertToNumericString(string input)
        {
            return string.Concat(input.Select(c => char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString()));
        }

        private static string FormatIban(string iban)
        {
            return string.Join(" ", Enumerable.Range(0, iban.Length / 4).Select(i => iban.Substring(i * 4, 4)));
        }
    }
}
