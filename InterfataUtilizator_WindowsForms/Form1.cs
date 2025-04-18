﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibrariiModeleBanking;
using Managers;

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
        private List<Card> carduri;

        private TextBox txtNume;
        private TextBox txtPrenume;
        private TextBox txtEmail;
        private TextBox txtParola;
        private ComboBox cmbTipCont;
        private ComboBox cmbBanca;
        private Button btnAdaugaCont;

        private TextBox[] txtNumarCard;
        private TextBox[] txtCVV;
        private TextBox[] txtDataExpirare;
        private TextBox[] txtPIN;
        private TextBox[] txtSold;
        private TextBox[] txtMoneda;
        private Button[] btnAdaugaCard;

        private bool isLoggedIn = false;
        private ContBancar contLogat = null;

        
        private Panel pnlLogin;
        private TextBox txtLoginEmail;
        private TextBox txtLoginParola;
        private Button btnLogin;
        private Button btnCreate;
        private Button btnDelogare;

        //private List<ContBancar> conturi;

        public Form1(List<ContBancar> conturi)
        {
            InitializeComponent();
            this.conturi = conturi ?? new List<ContBancar>();
            SetupLoginForm();
            this.Controls.Clear();
            this.Controls.Add(pnlLogin);

        }
        private void SetupLoginForm()
        {
            this.AutoScroll = false;
            pnlLogin = new Panel
            {
                Size = new Size(300, 250),
                Location = new Point((this.Width - 333) / 2, (this.Height - 300) / 2),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };
            

            Label lblTitle = new Label
            {
                Text = "Logare",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(120, 20)
            };

            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(30, 70),
                AutoSize = true
            };

            txtLoginEmail = new TextBox
            {
                Location = new Point(100, 70),
                Width = 150
            };

            Label lblParola = new Label
            {
                Text = "Parola:",
                Location = new Point(30, 110),
                AutoSize = true
            };

            txtLoginParola = new TextBox
            {
                Location = new Point(100, 110),
                Width = 150,
                PasswordChar = '*'
            };

            btnLogin = new Button
            {
                Text = "Loghează-te",
                Location = new Point(100, 150),
                Size = new Size(100, 30),
                BackColor = Color.LightBlue
            };
            btnCreate = new Button
            {
                Text = "Creaza-Cont",
                Location = new Point(100, 200),
                Size = new Size(100, 30),
                BackColor = Color.LightBlue
            };
            btnCreate.Click += Create;
            btnLogin.Click += BtnLogin_Click;

            pnlLogin.Controls.Add(lblTitle);
            pnlLogin.Controls.Add(lblEmail);
            pnlLogin.Controls.Add(txtLoginEmail);
            pnlLogin.Controls.Add(lblParola);
            pnlLogin.Controls.Add(txtLoginParola);
            pnlLogin.Controls.Add(btnLogin);
            pnlLogin.Controls.Add(btnCreate);
        }
        private void Create(object sender, EventArgs e)
        {
            this.Size = new Size(700, 600);
            this.Controls.Remove(pnlLogin);
            Panel addPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(this.ClientSize.Width - 30, 160),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.AliceBlue
            };
            this.Controls.Add(addPanel);

           
            SetupAddAccountControls(addPanel);

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
            

        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtLoginEmail.Text;
            string parola = txtLoginParola.Text;

            contLogat = conturi.FirstOrDefault(c => c.Email == email && c.Parola == parola);

            if (contLogat != null)
            {
                isLoggedIn = true;
                this.Controls.Remove(pnlLogin);
                
                MessageBox.Show($"Bun venit, {contLogat.Nume} {contLogat.Prenume}!", "Logare reușită");
                SetupForm();
                

                btnCreate.Visible = false;
            }
            else
            {
                MessageBox.Show("Email sau parolă incorecte!", "Eroare la logare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLoginParola.Clear();
            }
        }

        private void SetupForm()
        {
            this.Text = $"Bine ati venit - {contLogat.Nume} {contLogat.Prenume}";
            this.Size = new Size(700, 600);
            this.AutoScroll = true;
            this.BackColor = Color.White;

            if (contLogat.Id != 0) 
            {
                MeniuUtilizator();
            }
            else 
            {
                ShowAdminMenu();
                //AfiseazaConturiAdmin();
            }
        }

        private void AddBackToMenuButtonUtilizator(int x)
        {
            Button btnBack = new Button
            {
                Text = "Înapoi la Meniu",
                Location = new Point(10, 200 + x),
                Size = new Size(120, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, e) => {
                this.Controls.Clear();
                MeniuUtilizator();
            };
            this.Controls.Add(btnBack);
        }

        private void MeniuUtilizator()
        {
            this.Controls.Clear();
            this.Text = "Panou Utilizator";
            this.Size = new Size(500, 400);

            Label lblTitle = new Label
            {
                Text = "Meniu Utilizator",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(150, 50)
            };
            this.Controls.Add(lblTitle);


            Button btnViewAll = new Button
            {
                Text = "Vizualizare Carduri",
                Location = new Point(150, 120),
                Size = new Size(200, 40),
                BackColor = Color.LightBlue
            };
            btnViewAll.Click += (s, e) => {
                this.Controls.Clear();
                AfiseazaConturiSiCarduri(contLogat.Id);
                AddBackToMenuButtonUtilizator(100);
            };
            this.Controls.Add(btnViewAll);


            Button btnAddCard = new Button
            {
                Text = "Adăugare Card Nou",
                Location = new Point(150, 180),
                Size = new Size(200, 40),
                BackColor = Color.LightGreen
            };
            btnAddCard.Click += (s, e) => {
                this.Controls.Clear();
                SetupAddCardControls(contLogat.Id);
            };
            this.Controls.Add(btnAddCard);

            Button btnDelete = new Button
            {
                Text = "Stergere Card",
                Location = new Point(150, 240),
                Size = new Size(200, 40),
                BackColor = Color.Red
            };
            btnDelete.Click += (s, e) => {
                this.Controls.Clear();
                SetupAnulareCard(contLogat.Id);
                
            };
            this.Controls.Add(btnDelete);

            Button btnLogout = new Button
            {
                Text = "Delogare",
                Location = new Point(150, 300),
                Size = new Size(200, 40),
                BackColor = Color.LightCoral
            };
            btnLogout.Click += BtnDelogare_Click;
            this.Controls.Add(btnLogout);
        }

        private void SetupAnulareCard(int idCont)
        {
            this.Controls.Clear();

            Label lblHeader = new Label
            {
                Text = "Anulare Card",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblHeader);

            Label lblCvv = new Label
            {
                Text = "Introduceți CVV-ul cardului:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblCvv);

            TextBox txtCvv = new TextBox
            {
                Location = new Point(20, 90),
                Width = 100,
                MaxLength = 3,
                PasswordChar = '*'
            };
            this.Controls.Add(txtCvv);

            Button btnAnuleaza = new Button
            {
                Text = "Anulează Card",
                Location = new Point(20, 130),
                Size = new Size(120, 30),
                BackColor = Color.LightCoral
            };
            btnAnuleaza.Click += (s, e) =>
            {
                if (txtCvv.Text.Length != 3 || !txtCvv.Text.All(char.IsDigit))
                {
                    MessageBox.Show("CVV invalid! Introduceți exact 3 cifre.");
                    return;
                }

                AnulareCard(idCont, txtCvv.Text);
            };
            this.Controls.Add(btnAnuleaza);

            Button btnBack = new Button
            {
                Text = "Înapoi",
                Location = new Point(150, 130),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, e) => MeniuUtilizator();
            this.Controls.Add(btnBack);
        }

        public bool AnulareCard(int idCont, string cvv)
        {
            try
            {
                var cont = conturi.FirstOrDefault(c => c.Id == idCont);
                if (cont == null)
                {
                    MessageBox.Show("Contul nu există!");
                    return false;
                }

                var card = cont.carduri.FirstOrDefault(c => c.CVV.ToString() == cvv);
                if (card == null)
                {
                    MessageBox.Show("Nu există niciun card cu acest CVV în cont!");
                    return false;
                }

                var confirmResult = MessageBox.Show(
                    $"Sigur doriți să anulați cardul cu numărul {FormatCardNumber(card.NumarCard)}?",
                    "Confirmare anulare card",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes)
                    return false;

                cont.carduri.Remove(card);

                ActualizeazaFisierCarduri();

                MessageBox.Show("Card anulat cu succes!");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la anulare card: {ex.Message}");
                return false;
            }
        }

        private void SetupAddCardControls(int idCont)
        {
            this.Controls.Clear();

            Label lblTitle = new Label
            {
                Text = "Adăugare Card Nou",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            Label lblPin = new Label
            {
                Text = "PIN (4 cifre):",
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblPin);

            TextBox txtPin = new TextBox
            {
                Location = new Point(120, 60),
                Width = 100,
                MaxLength = 4,
                PasswordChar = '*'
            };
            this.Controls.Add(txtPin);

            Label lblSold = new Label
            {
                Text = "Sold inițial (min 100):",
                Location = new Point(20, 100),
                AutoSize = true
            };
            this.Controls.Add(lblSold);

            TextBox txtSold = new TextBox
            {
                Location = new Point(120, 100),
                Width = 100,
                Text = "100"
            };
            this.Controls.Add(txtSold);

            Label lblMoneda = new Label
            {
                Text = "Monedă:",
                Location = new Point(20, 140),
                AutoSize = true
            };
            this.Controls.Add(lblMoneda);

            ComboBox cmbMoneda = new ComboBox
            {
                Location = new Point(120, 140),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMoneda.Items.AddRange(new string[] { "RON", "EUR", "USD" });
            cmbMoneda.SelectedIndex = 0;
            this.Controls.Add(cmbMoneda);

            Button btnAdd = new Button
            {
                Text = "Adaugă Card",
                Location = new Point(20, 180),
                Size = new Size(100, 30),
                BackColor = Color.LightGreen
            };
            btnAdd.Click += (s, e) =>
            {
                if (!int.TryParse(txtPin.Text, out int pin) || txtPin.Text.Length != 4)
                {
                    MessageBox.Show("PIN invalid! Introduceți exact 4 cifre.");
                    return;
                }

                if (!double.TryParse(txtSold.Text, out double sold) || sold < 100)
                {
                    MessageBox.Show("Sold invalid! Minim 100.");
                    return;
                }

                AdaugaCard(idCont, txtPin.Text, sold, cmbMoneda.SelectedItem.ToString());
            };
            this.Controls.Add(btnAdd);

            Button btnBack = new Button
            {
                Text = "Înapoi",
                Location = new Point(130, 180),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, e) => MeniuUtilizator();
            this.Controls.Add(btnBack);
        }
        private void AdaugaCard(int idCont, string pin, double sold, string moneda)
        {
            try
            {
               
                var cont = conturi.FirstOrDefault(c => c.Id == idCont);
                if (cont == null)
                {
                    MessageBox.Show("Contul nu există!");
                    return;
                }

                
                int cvv = new Random().Next(100, 1000);
                DateTime dataExpirare = DateTime.Now.AddYears(4);
                string numarCard = CreateUnique16DigitString();
             
                Card cardNou = new Card(
                    id: idCont,       
                    CVV: cvv,
                    DataExpirare: dataExpirare,
                    NumarCard: numarCard,
                    Pin: pin,
                    sold: sold,
                    moneda: moneda
                );

                
                cont.carduri.Add(cardNou);

                
                ActualizeazaFisierCarduri();

                MessageBox.Show($"Card adăugat cu succes! ID Card: {idCont}");
                MeniuUtilizator();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adăugare card: {ex.Message}");
            }
        }

        private void ActualizeazaFisierCarduri()
        {
            string caleFisier = @"..\\..\\..\\carduri.txt";

            var toateCardurile = conturi
                .SelectMany(c => c.carduri)
                .OrderBy(c => c.Id) 
                .ToList();

            using (StreamWriter writer = new StreamWriter(caleFisier, false))
            {
                foreach (var card in toateCardurile)
                {
                    writer.WriteLine($"{card.Id};{card.NumarCard};{card.CVV};{card.DataExpirare:dd/MM/yyyy};{card.PIN};{card.SoldInitial};{card.Moneda}");
                }
            }
        }

        private void AfiseazaConturiAdmin()
        {
            int startY = 10;

            Panel contentPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(this.ClientSize.Width - 30, this.ClientSize.Height - 100),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(contentPanel);

            Label lblIdHeader = new Label { Text = "ID", Location = new Point(10, 10), Font = new Font("Arial", 10, FontStyle.Bold) };
            Label lblNumeHeader = new Label { Text = "Nume", Location = new Point(60, 10), Font = new Font("Arial", 10, FontStyle.Bold) };

            contentPanel.Controls.Add(lblIdHeader);
            contentPanel.Controls.Add(lblNumeHeader);

            startY = 40;
            int ok = 1;

            foreach (var cont in conturi)
            {
                
                Panel contPanel = new Panel
                {
                    Location = new Point(10, startY),
                    Size = new Size(contentPanel.Width - 40, 100), 
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White
                };

                
                Label lblId = new Label { Text = cont.Id.ToString(), Location = new Point(10, 10), AutoSize = true };
                Label lblNume = new Label { Text = cont.Nume, Location = new Point(60, 10), AutoSize = true };
                contPanel.Controls.Add(lblId);
                contPanel.Controls.Add(lblNume);

                int cardY = 35;

                if (cont.carduri.Count > 0)
                {
                    foreach (var card in cont.carduri)
                    {
                        Label lblCard = new Label
                        {
                            Text = $"• Card: {FormatCardNumber(card.NumarCard)} | Exp: {card.DataExpirare:MM/yyyy} | Sold: {card.SoldInitial} {card.Moneda}",
                            Location = new Point(20, cardY),
                            AutoSize = true,
                            ForeColor = Color.DarkGreen
                        };
                        contPanel.Controls.Add(lblCard);
                        cardY += 25;
                    }
                }
                else
                {
                    Label lblNoCards = new Label
                    {
                        Text = "Nu există carduri asociate",
                        Location = new Point(20, cardY),
                        AutoSize = true,
                        ForeColor = Color.Gray
                    };
                    contPanel.Controls.Add(lblNoCards);
                    cardY += 25;
                }
                

                contPanel.Height = cardY + 10;
                contentPanel.Controls.Add(contPanel);
                startY += contPanel.Height + 10;
               
            }
            

        }
        private static HashSet<string> Results = new HashSet<string>();
        private static Random RNG = new Random();
        private string Create16DigitString()
        {
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10).ToString());
            }
            return builder.ToString();
        }
        private string CreateUnique16DigitString()
        {
            var result = Create16DigitString();
            while (!Results.Add(result))
            {
                result = Create16DigitString();
            }

            return result;
        }

        private void BtnDelogare_Click(object sender, EventArgs e)
        {
           
            var result = MessageBox.Show("Sigur doriți să vă delogați?", "Confirmare delogare",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                
                isLoggedIn = false;
                contLogat = null;

                
                this.Controls.Clear();

                
                SetupLoginForm();
                this.Controls.Add(pnlLogin);

                
                txtLoginEmail.Text = "";
                txtLoginParola.Text = "";
            }
        }

        private void AfiseazaConturiSiCarduri(int idContLogat)
        {
            this.AutoScroll = true;
            this.AutoScrollMargin = new Size(0, 20);
            this.AutoScrollMinSize = new Size(800, 1000);

            
            foreach (var control in this.Controls.OfType<Control>().Where(c => c.Tag?.ToString() == "accountDisplay").ToList())
            {
                this.Controls.Remove(control);
            }

            
            
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel && panel.BackColor == Color.AliceBlue)
                {
                    panel.Visible = false;
                    break;
                }
            }
            int startY = idContLogat == 0 ? 50 : 20; 

            if (conturi == null || conturi.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Nu există conturi de afișat", Location = new Point(20, startY), AutoSize = true };
                this.Controls.Add(lblEmpty);
                return;
            }

            
            var conturiDeAfisat = idContLogat == 0
                ? conturi
                : conturi.Where(c => c.Id == idContLogat).ToList();

            foreach (var cont in conturiDeAfisat)
            {
                
                if (idContLogat == 0) 
                {
                    
                }
                else 
                {
                    var lblInfoCont = new Label
                    {
                        Text = $"Contul tău: {cont.Nume} {cont.Prenume} ({cont.TipCont}) - IBAN: {cont.IBAN}",
                        Location = new Point(20, startY),
                        AutoSize = true,
                        Font = new Font("Arial", 10, FontStyle.Bold)
                    };
                    this.Controls.Add(lblInfoCont);
                    startY += 30;


                    if (cont.carduri.Count > 0)
                    {
                        foreach (var card in cont.carduri)
                        {
                            string cardInfo = $"• Card: {FormatCardNumber(card.NumarCard)} " +
                                            $"Exp: {card.DataExpirare:MM/yyyy} " +
                                            $"Sold: {card.SoldInitial} {card.Moneda}";

                            Label lblCard = new Label
                            {
                                Text = cardInfo,
                                Location = new Point(40, startY),
                                ForeColor = Color.DarkBlue,
                                AutoSize = true
                            };
                            this.Controls.Add(lblCard);
                            startY += 25;
                        }
                    }
                    else
                    {
                        Label lblNoCards = new Label
                        {
                            Text = "Nu există carduri asociate",
                            Location = new Point(40, startY + 5),
                            ForeColor = Color.Gray,
                            Font = new Font("Arial", 8, FontStyle.Italic)
                        };
                        this.Controls.Add(lblNoCards);
                        startY += 25;
                    }
                }

                             
            }
            //if (idContLogat != 0)
            //{
            //    btnDelogare = new Button
            //    {
            //        Text = "Delogare",
            //        Location = new Point(250, 200),
            //        Size = new Size(100, 30),
            //        BackColor = Color.LightCoral,
            //    };
            //    btnDelogare.Click += BtnDelogare_Click;
            //    this.Controls.Add(btnDelogare);
            //}
        }

        private void SetupAddAccountControls(Panel container)
        {
            Label lblAdaugaCont = new Label
            {
                Text = "Adaugă Cont Nou",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            container.Controls.Add(lblAdaugaCont);

            // Nume
            Label lblNume = new Label { Text = "Nume:", Location = new Point(10, 40), AutoSize = true };
            txtNume = new TextBox { Location = new Point(100-10, 50-10), Width = 150 };
            container.Controls.Add(lblNume);
            container.Controls.Add(txtNume);

            // Prenume
            Label lblPrenume = new Label { Text = "Prenume:", Location = new Point(20 - 10, 80 - 10), AutoSize = true };
            txtPrenume = new TextBox { Location = new Point(100 - 10, 80 - 10), Width = 150 };
            container.Controls.Add(lblPrenume);
            container.Controls.Add(txtPrenume);

            // Email
            Label lblEmail = new Label { Text = "Email:", Location = new Point(20 - 10, 110 - 10), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(100 - 10, 110 - 10), Width = 150 };
            container.Controls.Add(lblEmail);
            container.Controls.Add(txtEmail);

            // Parola
            Label lblParola = new Label { Text = "Parola:", Location = new Point(270, 140 - 100), AutoSize = true };
            txtParola = new TextBox { Location = new Point(340, 40), Width = 150, PasswordChar = '*' };
            container.Controls.Add(lblParola);
            container.Controls.Add(txtParola);

            // Tip Cont
            Label lblTipCont = new Label { Text = "Tip Cont:", Location = new Point(270, 70), AutoSize = true };
            cmbTipCont = new ComboBox
            {
                Location = new Point(340, 70),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTipCont.Items.AddRange(new string[] { "ContCurent", "ContEconomii","ContInvestitii", "ContFirma" });
            cmbTipCont.SelectedIndex = 0;
            container.Controls.Add(lblTipCont);
            container.Controls.Add(cmbTipCont);

            // Banca
            Label lblBanca = new Label { Text = "Banca:", Location = new Point(270, 100), AutoSize = true };
            cmbBanca = new ComboBox
            {
                Location = new Point(340, 100),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbBanca.Items.AddRange(new string[] { "BTRL", "BNDR", "RFSN", "INGr" } );
            cmbBanca.SelectedIndex = 0;
            container.Controls.Add(lblBanca);
            container.Controls.Add(cmbBanca);

            btnAdaugaCont = new Button
            {
                Text = "Adaugă Cont",
                Location = new Point(510, 70),
                Size = new Size(100, 30),
                BackColor = Color.LightGreen
            };
            btnAdaugaCont.Click += BtnAdaugaCont_Click;
            container.Controls.Add(btnAdaugaCont);
            
        }

        private void BtnAdaugaCont_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNume.Text) ||
                string.IsNullOrWhiteSpace(txtPrenume.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtParola.Text))
            {
                MessageBox.Show("Toate câmpurile sunt obligatorii!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                
                int newId = conturi.Count > 0 ? conturi.Max(c => c.Id) + 1 : 1;

                
                ContBancar newCont = new ContBancar(
                    id: newId,
                    nume: txtNume.Text,
                    prenume: txtPrenume.Text,
                    email: txtEmail.Text,
                    parola: txtParola.Text,
                    tipcont: cmbTipCont.SelectedItem.ToString(),
                    banca: cmbBanca.SelectedItem.ToString(),
                    ib: 0 
                );

                
                conturi.Add(newCont);
                ManagerCont.SalveazaConturiInFisier(@"..\\..\\..\\conturi.txt", conturi);
                
                txtNume.Clear();
                txtPrenume.Clear();
                txtEmail.Clear();
                txtParola.Clear();
                cmbTipCont.SelectedIndex = 0;
                cmbBanca.SelectedIndex = 0;

                RefreshAccountDisplay();

                MessageBox.Show("Cont adăugat cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la adăugarea contului: {ex.Message}", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RefreshAccountDisplay()
        {

            this.Controls.Clear();
            if (isLoggedIn)
            {
                SetupForm();
                AfiseazaConturiSiCarduri(contLogat.Id);
            }
            else
            {
                SetupLoginForm();
                this.Controls.Add(pnlLogin);
            }
        }

        private void ShowAdminMenu()
        {
            this.Controls.Clear();
            this.Text = "Panou Administrator";
            this.Size = new Size(500, 400);

            Label lblTitle = new Label
            {
                Text = "Meniu Administrator",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(150, 50)
            };
            this.Controls.Add(lblTitle);

            
            Button btnViewAll = new Button
            {
                Text = "Vizualizare Conturi",
                Location = new Point(150, 120),
                Size = new Size(200, 40),
                BackColor = Color.LightBlue
            };
            btnViewAll.Click += (s, e) => {
                this.Controls.Clear();
                AfiseazaConturiAdmin();
                AddBackToMenuButton(100);
            };
            this.Controls.Add(btnViewAll);

            
            Button btnAddAccount = new Button
            {
                Text = "Adăugare Cont Nou",
                Location = new Point(150, 180),
                Size = new Size(200, 40),
                BackColor = Color.LightGreen
            };
            btnAddAccount.Click += (s, e) => {
                this.Controls.Clear();
                Create(s, e); 
                AddBackToMenuButton(0);
            };
            this.Controls.Add(btnAddAccount);

            Button btnDelete = new Button
            {
                Text = "Stergere Cont",
                Location = new Point(150, 240),
                Size = new Size(200, 40),
                BackColor = Color.Red
            };
            btnDelete.Click += (s, e) => {
                this.Controls.Clear();
                Delete(s, e);
                AddBackToMenuButton(0);
            };
            this.Controls.Add(btnDelete);

            Button btnLogout = new Button
            {
                Text = "Delogare",
                Location = new Point(150, 300),
                Size = new Size(200, 40),
                BackColor = Color.LightCoral
            };
            btnLogout.Click += BtnDelogare_Click;
            this.Controls.Add(btnLogout);
        }

        private void Delete(object sender, EventArgs e)
        {
            Label lblEmail = new Label
            {
                Text = "Introduceți emailul contului:",
                Location = new Point(10, 50),
                AutoSize = true
            };
            this.Controls.Add(lblEmail);

            // Adaugă TextBox pentru email
            TextBox txtEmailToDelete = new TextBox
            {
                Location = new Point(10, 80),
                Width = 200
            };
            this.Controls.Add(txtEmailToDelete);
            Button btnConfirmDelete = new Button
            {
                Text = "Confirmă ștergerea",
                Location = new Point(150, 170),
                Size = new Size(150, 30),
                BackColor = Color.LightCoral
            };
            btnConfirmDelete.Click += (s, ev) => ConfirmDelete(txtEmailToDelete.Text);
            this.Controls.Add(btnConfirmDelete);
        }
        private void ConfirmDelete(string email)
        {
            
            this.Controls.Clear();

            var contToDelete = conturi.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (contToDelete == null)
            {
                MessageBox.Show("Nu există niciun cont cu acest email!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowAdminMenu();
                return;
            }

            Label lblAccountInfo = new Label
            {
                Text = $"Cont selectat: {contToDelete.Nume} {contToDelete.Prenume} ({contToDelete.Email})",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblAccountInfo);

            Label lblPassword = new Label
            {
                Text = "Introduceți parola contului:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblPassword);

            TextBox txtPassword = new TextBox
            {
                Location = new Point(20, 90),
                Width = 200,
                PasswordChar = '*'
            };
            this.Controls.Add(txtPassword);

            
            Button btnConfirm = new Button
            {
                Text = "Confirmă ștergerea",
                Location = new Point(20, 130),
                Size = new Size(150, 30),
                BackColor = Color.LightCoral
            };
            btnConfirm.Click += (s, e) =>
            {
                if (txtPassword.Text == contToDelete.Parola)
                {
                    
                    var result = MessageBox.Show($"Sigur doriți să ștergeți definitiv contul {contToDelete.Nume} {contToDelete.Prenume}?",
                                              "Confirmare ștergere definitivă",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        
                        contToDelete.carduri.Clear();

                        StergeCardurileContului(contToDelete.Id);
                        conturi.Remove(contToDelete);


                        ManagerCont.SalveazaConturiInFisier(@"..\\..\\..\\conturi.txt", conturi);

                        MessageBox.Show("Cont și carduri asociate au fost șterse cu succes!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ShowAdminMenu();
                    }
                }
                else
                {
                    MessageBox.Show("Parolă incorectă!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Controls.Add(btnConfirm);
        }

        private void StergeCardurileContului(int idCont)
        {
            
            string caleFisierCarduri = @"..\\..\\..\\carduri.txt";

           
            List<string> liniiCarduri = File.ReadAllLines(caleFisierCarduri).ToList();

            
            List<string> liniiRamase = new List<string>();

            foreach (string linie in liniiCarduri)
            {
                
                string[] parts = linie.Split(';');
                if (parts.Length > 0 && int.Parse(parts[0]) != idCont)
                {
                    liniiRamase.Add(linie);
                }
            }

           
            File.WriteAllLines(caleFisierCarduri, liniiRamase);

            
            var cont = conturi.FirstOrDefault(c => c.Id == idCont);
            if (cont != null && cont.carduri != null)
            {
                cont.carduri.Clear();
            }
        }

        private void AddBackToMenuButton(int x)
        {
            Button btnBack = new Button
            {
                Text = "Înapoi la Meniu",
                Location = new Point(10, 200+x),
                Size = new Size(120, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, e) => {
                this.Controls.Clear();
                ShowAdminMenu();
            };
            this.Controls.Add(btnBack);
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