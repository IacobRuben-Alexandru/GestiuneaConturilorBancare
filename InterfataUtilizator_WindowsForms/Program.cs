using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using LibrariiModeleBanking;
using Managers;

namespace InterfataUtilizator_WindowsForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<ContBancar> conturi = ManagerCont.CitesteConturiDinFisier(@"..\\..\\..\\conturi.txt", @"..\\..\\..\\carduri.txt");
            Application.Run(new Form1(conturi));
        }
    }
}