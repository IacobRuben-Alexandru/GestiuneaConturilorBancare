using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;
            this.conturi = conturi ?? new List<ContBancar>();
            SetupLoginForm();
            this.Controls.Clear();
            this.Controls.Add(pnlLogin);

        }
        private void SetupLoginForm()
        {
            this.Text = "Logare Cont Bancar";
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;
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
        private void Create(object sender, EventArgs e)
        {
            this.Text = "Creare Cont Bancar";
            this.Size = new Size(700, 600);
            this.Controls.Remove(pnlLogin);
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;
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

        private void SetupAddAccountControls(Panel container)
        {
            this.Text = "Adaugă Cont Nou";
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;
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
            txtNume = new TextBox { Location = new Point(100 - 10, 50 - 10), Width = 150 };
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
            cmbTipCont.Items.AddRange(new string[] { "ContCurent", "ContEconomii", "ContInvestitii", "ContFirma" });
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
            cmbBanca.Items.AddRange(new string[] { "BTRL", "BNDR", "RFSN", "INGr" });
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
            string nume = txtNume.Text.Trim();
            string prenume = txtPrenume.Text.Trim();
            string email = txtEmail.Text.Trim();
            string parola = txtParola.Text;

            if (string.IsNullOrWhiteSpace(nume) ||
                string.IsNullOrWhiteSpace(prenume) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(parola))
            {
                MessageBox.Show("Toate câmpurile sunt obligatorii!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(nume, @"^[A-Za-zĂÎÂȘȚăîâșț\- ]{2,}$") || !Regex.IsMatch(prenume, @"^[A-Za-zĂÎÂȘȚăîâșț\- ]{2,}$"))
            {
                MessageBox.Show("Numele și prenumele trebuie să conțină doar litere și să aibă cel puțin 2 caractere.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                MessageBox.Show("Email-ul trebuie să fie valid și să aparțină domeniului @gmail.com.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (parola.Length < 6 || !Regex.IsMatch(parola, @"[A-Za-z]") || !Regex.IsMatch(parola, @"\d"))
            {
                MessageBox.Show("Parola trebuie să aibă minim 6 caractere și să conțină cel puțin o literă și o cifră.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int newId = conturi.Count > 0 ? conturi.Max(c => c.Id) + 1 : 1;

                ContBancar newCont = new ContBancar(
                    id: newId,
                    nume: nume,
                    prenume: prenume,
                    email: email,
                    parola: parola,
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
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;
            this.Size = new Size(600, 550);
            this.BackColor = ColorTranslator.FromHtml("#F5F5F5");

            Label lblTitle = new Label
            {
                Text = "Meniu Utilizator",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#333333"),
                Location = new Point(200, 30)
            };
            this.Controls.Add(lblTitle);

            int btnWidth = 220;
            int btnHeight = 45;
            int btnLeft = 190;
            int spacing = 20;
            int startY = 100;

            Button btnViewAll = new Button
            {
                Text = "Vizualizare Carduri",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY),
                BackColor = ColorTranslator.FromHtml("#007ACC"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
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
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#4CAF50"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnAddCard.Click += (s, e) => {
                this.Controls.Clear();
                SetupAddCardControls(contLogat.Id);
            };
            this.Controls.Add(btnAddCard);

            Button btnGestionare = new Button
            {
                Text = "Gestionare Card",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + 2 * (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#9C27B0"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnGestionare.Click += (s, e) => {
                this.Controls.Clear();
                SetupGestionare(contLogat.Id);
            };
            this.Controls.Add(btnGestionare);

            Button btnDelete = new Button
            {
                Text = "Stergere Card",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + 3 * (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#E53935"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnDelete.Click += (s, e) => {
                this.Controls.Clear();
                SetupAnulareCard(contLogat.Id);
            };
            this.Controls.Add(btnDelete);

            Button btnLogout = new Button
            {
                Text = "Delogare",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + 4 * (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#757575"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnLogout.Click += BtnDelogare_Click;
            this.Controls.Add(btnLogout);
        }





        private void ShowAdminMenu()
        {
            this.Controls.Clear();
            this.Text = "Panou Administrator";
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;
            this.Size = new Size(600, 500);
            this.BackColor = ColorTranslator.FromHtml("#F5F5F5");

            Label lblTitle = new Label
            {
                Text = "Meniu Administrator",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                ForeColor = ColorTranslator.FromHtml("#333333"),
                Location = new Point(180, 30)
            };
            this.Controls.Add(lblTitle);

            int btnWidth = 220;
            int btnHeight = 45;
            int btnLeft = 190;
            int spacing = 20;
            int startY = 100;

            Button btnViewAll = new Button
            {
                Text = "Vizualizare Conturi",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY),
                BackColor = ColorTranslator.FromHtml("#007ACC"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnViewAll.Click += (s, e) => {
                this.Controls.Clear();
                AfiseazaConturiAdmin();
                AddBackToMenuButton(180);
            };
            this.Controls.Add(btnViewAll);

            Button btnAddAccount = new Button
            {
                Text = "Adăugare Cont Nou",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#4CAF50"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
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
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + 2 * (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#E53935"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnDelete.Click += (s, e) => {
                this.Controls.Clear();
                Delete(s, e);
            };
            this.Controls.Add(btnDelete);

            Button btnLogout = new Button
            {
                Text = "Delogare",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(btnWidth, btnHeight),
                Location = new Point(btnLeft, startY + 3 * (btnHeight + spacing)),
                BackColor = ColorTranslator.FromHtml("#757575"),
                ForeColor = Color.White,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
            btnLogout.Click += BtnDelogare_Click;
            this.Controls.Add(btnLogout);
        }



        private void AddBackToMenuButton(int x)
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
                ShowAdminMenu();
            };
            this.Controls.Add(btnBack);
        }

        private void AfiseazaConturiAdmin()
        {
            this.Text = "Conturi Bancare";
            int startY = 10;
            this.Icon = InterfataUtilizator_WindowsForms.Properties.Resources.wallet_icon_16x16;

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
                            Text = $"• Card: {FormatCardNumber(card.NumarCard)} | Exp: {card.DataExpirare:MM/yyyy} | Sold: {card.SoldInitial} | Tip moneda: {card.Moneda} | Status: {card.EsteActiv}",
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

        private string FormatCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
                return cardNumber;

            return $"{cardNumber.Substring(0, 4)} {cardNumber.Substring(4, 4)} " +
                   $"{cardNumber.Substring(8, 4)} {cardNumber.Substring(12, 4)}";
        }

        private void AfiseazaConturiSiCarduri(int idContLogat)
        {
            this.Text = "Conturi și Carduri";
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

        private void SetupAddCardControls(int idCont)
        {
            this.Controls.Clear();
            this.Text = "Adăugare Card Nou";
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
                    moneda: moneda,
                    esteActiv: true
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
                    writer.WriteLine($"{card.Id};{card.NumarCard};{card.CVV};{card.DataExpirare:dd/MM/yyyy};{card.PIN};{card.SoldInitial};{card.Moneda};{(card.EsteActiv ? 1 : 0)}");
                }
            }
        }

        private void SetupAnulareCard(int idCont)
        {
            this.Controls.Clear();
            this.Text = "Stergere Card";
            Label lblHeader = new Label
            {
                Text = "Stergere Card",
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
                Text = "Stergere Card",
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

        private void SetupGestionare(int idCont)
        {
            this.Controls.Clear();
            this.Text = "Gestionare Carduri";
            this.AutoScroll = true;

            var cont = conturi.FirstOrDefault(c => c.Id == idCont);
            if (cont == null || cont.carduri.Count == 0)
            {
                Label lblNoCards = new Label
                {
                    Text = "Nu aveți niciun card asociat contului",
                    Location = new Point(20, 20),
                    AutoSize = true
                };
                this.Controls.Add(lblNoCards);
                AddBackButton();
                return;
            }

            Label lblTitle = new Label
            {
                Text = $"Gestionare Carduri - {cont.Nume} {cont.Prenume}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            ComboBox cmbCarduri = new ComboBox
            {
                Location = new Point(20, 60),
                Width = 500,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            foreach (var card in cont.carduri)
            {
                cmbCarduri.Items.Add($"Card: {FormatCardNumber(card.NumarCard)} | Exp: {card.DataExpirare:MM/yyyy} | Sold: {card.SoldInitial} {card.Moneda}");
            }

            cmbCarduri.SelectedIndex = 0;
            this.Controls.Add(cmbCarduri);

            int startY = 110;

            GroupBox groupInfo = new GroupBox
            {
                Text = "Informații Card",
                Location = new Point(20, startY),
                Size = new Size(580, 60)
            };

            Button btnDetalii = new Button
            {
                Text = "Detalii Card",
                Location = new Point(20, 20),
                Size = new Size(120, 30),
                BackColor = Color.LightBlue
            };
            btnDetalii.Click += (s, e) => ShowCardDetails(cont.carduri[cmbCarduri.SelectedIndex]);
            groupInfo.Controls.Add(btnDetalii);

            Button btnSchimbaPin = new Button
            {
                Text = "Schimbă PIN",
                Location = new Point(160, 20),
                Size = new Size(120, 30),
                BackColor = Color.LightGreen
            };
            btnSchimbaPin.Click += (s, e) => SetupSchimbaPin(cont.carduri[cmbCarduri.SelectedIndex]);
            groupInfo.Controls.Add(btnSchimbaPin);

            this.Controls.Add(groupInfo);

            startY += groupInfo.Height + 10;
            GroupBox groupStatus = new GroupBox
            {
                Text = "Status Card",
                Location = new Point(20, startY),
                Size = new Size(580, 60)
            };

            Button btnToggleStatus = new Button
            {
                Text = cont.carduri[cmbCarduri.SelectedIndex].EsteActiv ? "Blochează Card" : "Activează Card",
                Location = new Point(20, 20),
                Size = new Size(150, 30),
                BackColor = cont.carduri[cmbCarduri.SelectedIndex].EsteActiv ? Color.Orange : Color.LimeGreen,
                Tag = cmbCarduri.SelectedIndex
            };

            cmbCarduri.SelectedIndexChanged += (s, e) =>
            {
                btnToggleStatus.Text = cont.carduri[cmbCarduri.SelectedIndex].EsteActiv ? "Blochează Card" : "Activează Card";
                btnToggleStatus.BackColor = cont.carduri[cmbCarduri.SelectedIndex].EsteActiv ? Color.Orange : Color.LimeGreen;
                btnToggleStatus.Tag = cmbCarduri.SelectedIndex;
            };

            btnToggleStatus.Click += (s, e) =>
            {
                int selectedIndex = (int)btnToggleStatus.Tag;
                ToggleCardStatus(cont.carduri[selectedIndex], btnToggleStatus);
            };

            groupStatus.Controls.Add(btnToggleStatus);
            this.Controls.Add(groupStatus);

            startY += groupStatus.Height + 10;
            GroupBox groupOperatiuni = new GroupBox
            {
                Text = "Operațiuni Financiare",
                Location = new Point(20, startY),
                Size = new Size(580, 60)
            };

            Button btnDepunere = new Button
            {
                Text = "Depunere",
                Location = new Point(20, 20),
                Size = new Size(100, 30),
                BackColor = Color.LightGreen,
                Tag = cmbCarduri.SelectedIndex
            };
            btnDepunere.Click += (s, e) =>
            {
                int selectedIndex = cmbCarduri.SelectedIndex;
                ShowOperatiunePanel(cont.carduri[selectedIndex], "DEPUNERE", cont.Id);
            };
            groupOperatiuni.Controls.Add(btnDepunere);

            Button btnRetragere = new Button
            {
                Text = "Retragere",
                Location = new Point(130, 20),
                Size = new Size(100, 30),
                BackColor = Color.LightSalmon,
                Tag = cmbCarduri.SelectedIndex
            };
            btnRetragere.Click += (s, e) =>
            {
                int selectedIndex = cmbCarduri.SelectedIndex;
                ShowOperatiunePanel(cont.carduri[selectedIndex], "RETRAGERE", cont.Id);
            };
            groupOperatiuni.Controls.Add(btnRetragere);

            this.Controls.Add(groupOperatiuni);

            if (cont.carduri.Count > 1)
            {
                startY += groupOperatiuni.Height + 10;
                GroupBox groupTransfer = new GroupBox
                {
                    Text = "Transfer între Carduri",
                    Location = new Point(20, startY),
                    Size = new Size(580, 60)
                };

                Button btnTransfer = new Button
                {
                    Text = "Transfer",
                    Location = new Point(20, 20),
                    Size = new Size(100, 30),
                    BackColor = Color.LightSkyBlue,
                    Tag = cmbCarduri.SelectedIndex
                };
                btnTransfer.Click += (s, e) =>
                {
                    int selectedIndex = cmbCarduri.SelectedIndex;
                    SetupTransferIntreCarduri(cont, selectedIndex);
                };

                groupTransfer.Controls.Add(btnTransfer);
                this.Controls.Add(groupTransfer);
            }

            AddBackButton();
        }





        private void ShowOperatiunePanel(Card card, string tipOperatiune, int idCont)
        {
            this.Text = $"{tipOperatiune} Card";
            this.Controls.Clear();
            Panel pnlOperatiune = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(200, 150),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.AliceBlue
            };

            Label lblTitle = new Label
            {
                Text = tipOperatiune,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            NumericUpDown nudSuma = new NumericUpDown
            {
                Minimum = 10,
                Maximum = 10000,
                Increment = 10,
                Value = 100,
                Location = new Point(10, 40),
                Width = 150
            };

            Button btnConfirm = new Button
            {
                Text = "Confirmă",
                Location = new Point(10, 80),
                Size = new Size(80, 30),
                BackColor = Color.LightGreen,
                Tag = new Tuple<Card, string>(card, tipOperatiune)
            };
            btnConfirm.Click += (s, e) => ProceseazaOperatiune(
                ((Tuple<Card, string>)btnConfirm.Tag).Item1,
                (decimal)nudSuma.Value,
                ((Tuple<Card, string>)btnConfirm.Tag).Item2
            );
            Button btnBack = new Button
            {
                Text = "Înapoi",
                Location = new Point(105, 80),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, e) => SetupGestionare(idCont);
            this.Controls.Add(btnBack);
            pnlOperatiune.Controls.AddRange(new Control[] { lblTitle, nudSuma, btnConfirm, btnBack });
            this.Controls.Add(pnlOperatiune);

        }

        private void ProceseazaOperatiune(Card card, decimal suma, string tipOperatiune)
        {
            if (!card.EsteActiv)
            {
                MessageBox.Show("Cardul este blocat! Nu puteți efectua operații.");
                return;
            }
            try
            {
                switch (tipOperatiune)
                {
                    case "DEPUNERE":
                        card.SoldInitial += (double)suma;
                        break;

                    case "RETRAGERE":
                        if (card.SoldInitial >= (double)suma)
                        {
                            card.SoldInitial -= (double)suma;
                        }
                        else
                        {
                            MessageBox.Show("Fonduri insuficiente!");
                            return;
                        }
                        break;
                }

                ActualizeazaFisierCarduri();
                MessageBox.Show($"{tipOperatiune} realizată cu succes!\nNoul sold: {card.SoldInitial} {card.Moneda}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}");
            }
        }

        private void SetupTransferIntreCarduri(ContBancar cont, int indexCardSursa)
        {
            this.Controls.Clear();
            this.Text = "Transfer între Carduri";
            Label lblTitle = new Label
            {
                Text = "Transfer între carduri",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            Label lblSursa = new Label
            {
                Text = $"Din cardul: {FormatCardNumber(cont.carduri[indexCardSursa].NumarCard)}",
                Location = new Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblSursa);

            ComboBox cmbDestinatie = new ComboBox
            {
                Location = new Point(20, 90),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            for (int i = 0; i < cont.carduri.Count; i++)
            {
                if (i != indexCardSursa)
                {
                    cmbDestinatie.Items.Add(new { Index = i, Text = $"Card: {FormatCardNumber(cont.carduri[i].NumarCard)} ({cont.carduri[i].Moneda})" });
                }
            }
            cmbDestinatie.DisplayMember = "Text";
            cmbDestinatie.ValueMember = "Index";
            cmbDestinatie.SelectedIndex = 0;
            this.Controls.Add(cmbDestinatie);

            NumericUpDown nudSuma = new NumericUpDown
            {
                Minimum = 10,
                Maximum = 10000,
                Increment = 10,
                Value = 100,
                Location = new Point(20, 130),
                Width = 150
            };
            this.Controls.Add(nudSuma);

            Button btnTransfer = new Button
            {
                Text = "Efectuează transfer",
                Location = new Point(20, 170),
                Size = new Size(150, 30),
                BackColor = Color.LightBlue
            };
            btnTransfer.Click += (s, e) =>
            {
                var selected = (dynamic)cmbDestinatie.SelectedItem;
                ProceseazaTransfer(
                    cont.carduri[indexCardSursa],
                    cont.carduri[(int)selected.Index],
                    (decimal)nudSuma.Value
                );
            };
            this.Controls.Add(btnTransfer);
            ActualizeazaFisierCarduri();
            AddBackButton();
        }

        private void ProceseazaTransfer(Card sursa, Card destinatie, decimal suma)
        {
            if (sursa.Moneda != destinatie.Moneda)
            {
                MessageBox.Show("Transferul între monede diferite nu este permis!");
                return;
            }

            if (sursa.SoldInitial < (double)suma)
            {
                MessageBox.Show("Fonduri insuficiente în cardul sursă!");
                return;
            }

            sursa.SoldInitial -= (double)suma;
            destinatie.SoldInitial += (double)suma;

            ActualizeazaFisierCarduri();
            MessageBox.Show($"Transfer realizat cu succes!\nNoul sold: {sursa.SoldInitial} {sursa.Moneda}");
            SetupGestionare(sursa.Id);
        }

        private void ShowCardDetails(Card card)
        {
            string detalii = $"Număr card: {FormatCardNumber(card.NumarCard)}\n" +
                            $"Data expirare: {card.DataExpirare:MM/yyyy}\n" +
                            $"Monedă: {card.Moneda}\n" +
                            $"Sold curent: {card.SoldInitial} {card.Moneda}\n" +
                            $"Status: {(card.EsteActiv ? "Activ" : "Blocat")}";

            MessageBox.Show(detalii, "Detalii Card");
        }

        private void SetupSchimbaPin(Card card)
        {
            Form inputForm = new Form
            {
                Width = 300,
                Height = 230,
                Text = "Schimbare PIN"
            };

            TextBox txtPinVechi = new TextBox { Location = new Point(20, 20), Width = 100, PasswordChar = '*' };
            TextBox txtPinNou = new TextBox { Location = new Point(20, 60), Width = 100, PasswordChar = '*' };
            TextBox txtConfirmaPin = new TextBox { Location = new Point(20, 100), Width = 100, PasswordChar = '*' };

            inputForm.Controls.AddRange(new Control[]
            {
                new Label { Text = "PIN vechi:", Location = new Point(20, 0) },
                txtPinVechi,
                new Label { Text = "PIN nou:", Location = new Point(20, 40) },
                txtPinNou,
                new Label { Text = "Confirmă PIN:", Location = new Point(20, 80) },
                txtConfirmaPin,
                new Button {
                    Text = "Confirmă",
                    Location = new Point(150, 140),
                    DialogResult = DialogResult.OK
                 }
            });

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                if (txtPinVechi.Text != card.PIN)
                {
                    MessageBox.Show("PIN vechi incorect!");
                    return;
                }

                if (txtPinNou.Text != txtConfirmaPin.Text)
                {
                    MessageBox.Show("PIN-urile nu coincid!");
                    return;
                }
                bool succes = int.TryParse(txtPinNou.Text, out int pinNou);
                if (succes = false || pinNou <= pinVerif ||pinNou > pinVerifSup)
                {
                    MessageBox.Show("PIN invalid! Introduceți exact 4 cifre.");
                    return;
                }


                card.PIN = txtPinNou.Text;
                ActualizeazaFisierCarduri();
                MessageBox.Show("PIN schimbat cu succes!");
            }

        }
        public int pinVerif = 999;
        public int pinVerifSup = 9999;

        private void ToggleCardStatus(Card card, Button btnToggle)
        {
            card.EsteActiv = !card.EsteActiv;
            btnToggle.Text = card.EsteActiv ? "Blochează Card" : "Activează Card";
            btnToggle.BackColor = card.EsteActiv ? Color.Orange : Color.LimeGreen;
            MessageBox.Show($"Card {(card.EsteActiv ? "activat" : "blocat")} cu succes!");
            ActualizeazaFisierCarduri();
        }


        private void AddBackButton()
        {
            Button btnBack = new Button
            {
                Text = "Înapoi",
                Location = new Point(350, 20),
                Size = new Size(80, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, e) => MeniuUtilizator();
            this.Controls.Add(btnBack);
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

        private void Delete(object sender, EventArgs e)
        {
            this.Text = "Ștergere Cont";
            Label lblEmail = new Label
            {
                Text = "Introduceți emailul contului:",
                Location = new Point(10, 50),
                AutoSize = true
            };
            this.Controls.Add(lblEmail);

            TextBox txtEmailToDelete = new TextBox
            {
                Location = new Point(10, 80),
                Width = 200
            };
            this.Controls.Add(txtEmailToDelete);
            Button btnConfirmDelete = new Button
            {
                Text = "Confirmă ștergerea",
                Location = new Point(10, 130),
                Size = new Size(120, 30),
                BackColor = Color.LightCoral
            };
            Button btnBack = new Button
            {
                Text = "Înapoi la Meniu",
                Location = new Point(150, 130 ),
                Size = new Size(120, 30),
                BackColor = Color.LightGray
            };
            btnBack.Click += (s, ev) => ShowAdminMenu();
            this.Controls.Add(btnBack);
            btnConfirmDelete.Click += (s, ev) => ConfirmDelete(txtEmailToDelete.Text);
            this.Controls.Add(btnConfirmDelete);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ConfirmDelete(string email)
        {
            this.Text = "Ștergere Cont";
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



    }
}