using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LibrariiModeleBanking;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        // Etichete pentru antete
        private Label lblIdHeader;
        private Label lblNumeHeader;
        private Label lblPrenumeHeader;
        private Label lblTipContHeader;
        private Label lblIbanHeader;

        // Array-uri de etichete pentru date
        private Label[] lblsId;
        private Label[] lblsNume;
        private Label[] lblsPrenume;
        private Label[] lblsTipCont;
        private Label[] lblsIban;

        private List<ContBancar> conturi;

        public Form1(List<ContBancar> conturi)
        {
            InitializeComponent();
            this.conturi = conturi ?? new List<ContBancar>();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Lista Conturi Bancare";
            this.Size = new Size(700, 500);
            this.AutoScroll = true;

            // Inițializare antete
            lblIdHeader = new Label { Text = "ID", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            lblNumeHeader = new Label { Text = "Nume", Location = new Point(70, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            lblPrenumeHeader = new Label { Text = "Prenume", Location = new Point(170, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            lblTipContHeader = new Label { Text = "Tip Cont", Location = new Point(270, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            lblIbanHeader = new Label { Text = "IBAN", Location = new Point(370, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };

            this.Controls.Add(lblIdHeader);
            this.Controls.Add(lblNumeHeader);
            this.Controls.Add(lblPrenumeHeader);
            this.Controls.Add(lblTipContHeader);
            this.Controls.Add(lblIbanHeader);

            AfiseazaConturi();
        }

        private void AfiseazaConturi()
        {
            if (conturi == null || conturi.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Nu există conturi de afișat", Location = new Point(20, 50), AutoSize = true };
                this.Controls.Add(lblEmpty);
                return;
            }

            // Inițializare array-uri
            int count = conturi.Count;
            lblsId = new Label[count];
            lblsNume = new Label[count];
            lblsPrenume = new Label[count];
            lblsTipCont = new Label[count];
            lblsIban = new Label[count];

            int topPosition = 50;

            for (int i = 0; i < count; i++)
            {
                // ID
                lblsId[i] = new Label
                {
                    Text = conturi[i].Id.ToString(),
                    Location = new Point(20, topPosition),
                    AutoSize = true
                };

                // Nume
                lblsNume[i] = new Label
                {
                    Text = conturi[i].Nume,
                    Location = new Point(70, topPosition),
                    AutoSize = true
                };

                // Prenume
                lblsPrenume[i] = new Label
                {
                    Text = conturi[i].Prenume,
                    Location = new Point(170, topPosition),
                    AutoSize = true
                };

                // Tip Cont
                lblsTipCont[i] = new Label
                {
                    Text = conturi[i].TipCont,
                    Location = new Point(270, topPosition),
                    AutoSize = true
                };

                // IBAN
                lblsIban[i] = new Label
                {
                    Text = conturi[i].IBAN,
                    Location = new Point(370, topPosition),
                    AutoSize = true
                };

                // Adaugă toate etichetele pe formular
                this.Controls.Add(lblsId[i]);
                this.Controls.Add(lblsNume[i]);
                this.Controls.Add(lblsPrenume[i]);
                this.Controls.Add(lblsTipCont[i]);
                this.Controls.Add(lblsIban[i]);

                topPosition += 30;
            }
        }
    }
}