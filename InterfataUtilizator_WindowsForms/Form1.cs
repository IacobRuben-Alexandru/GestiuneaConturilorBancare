using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LibrariiModeleBanking;

namespace InterfataUtilizator_WindowsForms
{
    public partial class Form1 : Form
    {
        
        private Label lblIdHeader;
        private Label lblNumeHeader;
        private Label lblPrenumeHeader;
        private Label lblTipContHeader;
        private Label lblIbanHeader;

        
        private Label[] lblsId;
        private Label[] lblsNume;
        private Label[] lblsPrenume;
        private Label[] lblsTipCont;
        private Label[] lblsIban;
        private Label[] lblsCarduri;
        private List<ContBancar> conturi;

        public Form1(List<ContBancar> conturi)
        {
            InitializeComponent();
            this.conturi = conturi ?? new List<ContBancar>();
            SetupForm();
            AfiseazaConturiSiCarduri();
        }

        private void SetupForm()
        {
            this.Text = "Lista Conturi Bancare";
            this.Size = new Size(700, 500);
            this.AutoScroll = true;
            this.BackColor = Color.White;

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

            AfiseazaConturiSiCarduri();
        }

        private void AfiseazaConturiSiCarduri()
        {
            if (conturi == null || conturi.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Nu există conturi de afișat", Location = new Point(20, 50), AutoSize = true };
                this.Controls.Add(lblEmpty);
                return;
            }

            
            int count = conturi.Count;
            lblsId = new Label[count];
            lblsNume = new Label[count];
            lblsPrenume = new Label[count];
            lblsTipCont = new Label[count];
            lblsIban = new Label[count];
            lblsCarduri = new Label[count];
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
                if (conturi[i].carduri.Count > 0)
                {
                    foreach (var card in conturi[i].carduri)
                    {
                        string cardInfo = $"• Card: {FormatCardNumber(card.NumarCard)} " +
                                        $"Exp: {card.DataExpirare:MM/yyyy} " +
                                        $"Sold: {card.SoldInitial} {card.Moneda}";

                        Label lblCard = new Label
                        {
                            Text = cardInfo,
                            Location = new Point(40, topPosition),
                            ForeColor = Color.DarkBlue,
                            AutoSize = true
                        };
                        this.Controls.Add(lblCard);
                        topPosition += 20;
                    }
                }
                else
                {
                    Label lblNoCards = new Label
                    {
                        Text = "Nu există carduri asociate",
                        Location = new Point(40, topPosition),
                        ForeColor = Color.Gray,
                        Font = new Font("Arial", 8, FontStyle.Italic)
                    };
                    this.Controls.Add(lblNoCards);
                    topPosition += 20;
                }

                // Linie separator
                Panel separator = new Panel
                {
                    BackColor = Color.LightGray,
                    Height = 1,
                    Width = this.ClientSize.Width - 40,
                    Location = new Point(20, topPosition)
                };
                this.Controls.Add(separator);
                topPosition += 15;
            }
        }

        private string FormatCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
                return cardNumber;

            return $"{cardNumber.Substring(0, 4)} {cardNumber.Substring(4, 4)} " +
                   $"{cardNumber.Substring(8, 4)} {cardNumber.Substring(12, 4)}";
        }
        
    }
}