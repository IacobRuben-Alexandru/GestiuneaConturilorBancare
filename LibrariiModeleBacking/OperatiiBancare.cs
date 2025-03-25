using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrariiModeleBanking
{
    public class OperatiiBancare
    {
        public static void Depunere(double suma, Card card)
        {
            if (suma > 0)
            {
                card.SoldInitial += suma;
                Console.WriteLine($"Depunere reusita. Sold actual: {card.SoldInitial}");
            }
            else
            {
                Console.WriteLine("Suma introdusa este invalida.");
            }
        }
        public static void Retragere(double suma, Card card)
        {
            if (suma > 0 && suma <= card.SoldInitial)
            {
                card.SoldInitial -= suma;
                Console.WriteLine($"Retragere reusita. Sold actual: {card.SoldInitial}");
            }
            else
            {
                Console.WriteLine("Fonduri insuficiente sau suma invalida.");
            }
        }
        public static void Transfer(double suma, Card cardSursa, Card cardDestinatie)
        {
            if(suma > 0 && suma <= cardSursa.SoldInitial)
            {
                cardSursa.SoldInitial -= suma;
                cardDestinatie.SoldInitial += suma;
                Console.WriteLine($"Transfer reusit. Sold actual: {cardSursa.SoldInitial}");
            }
            else
            {
                Console.WriteLine("Fonduri insuficiente sau suma invalida.");
            }
        }

    }
}
