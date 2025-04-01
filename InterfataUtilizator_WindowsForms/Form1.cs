using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        //private List<ContBancar> conturi;

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

            Panel addPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(this.ClientSize.Width - 30, 160),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.AliceBlue
            };
            this.Controls.Add(addPanel);

            // Mută controalele de adăugare în panel
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

            AfiseazaConturiSiCarduri();
        }

        private void AfiseazaConturiSiCarduri()
        {
            foreach (var control in this.Controls.OfType<Control>().Where(c => c.Tag?.ToString() == "accountDisplay").ToList())
            {
                this.Controls.Remove(control);
            }

            int startY = lblIdHeader.Bottom + 150;

            if (conturi == null || conturi.Count == 0)
            {
                Label lblEmpty = new Label { Text = "Nu există conturi de afișat", Location = new Point(20, startY), AutoSize = true };
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
            //int topPosition = 50;

            for (int i = 0; i < count; i++)
            {
                // ID
                lblsId[i] = new Label
                {
                    Text = conturi[i].Id.ToString(),
                    Location = new Point(20, startY),
                    AutoSize = true
                };
                
                // Nume
                lblsNume[i] = new Label
                {
                    Text = conturi[i].Nume,
                    Location = new Point(70, startY),
                    AutoSize = true
                };
                
                // Prenume
                lblsPrenume[i] = new Label
                {
                    Text = conturi[i].Prenume,
                    Location = new Point(170, startY),
                    AutoSize = true
                };
                   
                // Tip Cont
                lblsTipCont[i] = new Label
                {
                    Text = conturi[i].TipCont,
                    Location = new Point(270, startY),
                    AutoSize = true
                };
                
                // IBAN
                lblsIban[i] = new Label
                {
                    Text = conturi[i].IBAN,
                    Location = new Point(370, startY),
                    AutoSize = true
                };
                startY += 30;
                // Adaugă toate etichetele pe formular
                this.Controls.Add(lblsId[i]);
                this.Controls.Add(lblsNume[i]);
                this.Controls.Add(lblsPrenume[i]);
                this.Controls.Add(lblsTipCont[i]);
                this.Controls.Add(lblsIban[i]);

                
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
                            Location = new Point(40, startY),
                            ForeColor = Color.DarkBlue,
                            AutoSize = true
                        };
                        startY = startY + 30;
                        this.Controls.Add(lblCard);
                      
                    }
                }
                else
                {
                    Label lblNoCards = new Label
                    {
                        Text = "Nu există carduri asociate",
                        Location = new Point(40, startY),
                        ForeColor = Color.Gray,
                        Font = new Font("Arial", 8, FontStyle.Italic)
                    };
                    startY += 30;
                    this.Controls.Add(lblNoCards);
                    
                }

                // Linie separator
                Panel separator = new Panel
                {
                    BackColor = Color.LightGray,
                    Height = 1,
                    Width = this.ClientSize.Width - 40,
                    Location = new Point(20, startY)
                };
                startY += 30;
                this.Controls.Add(separator);
                
            }
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
            txtEmail = new TextBox { Location = new Point(100 - 10, 110 - 10), Width = 200 };
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
            
            foreach (var control in this.Controls.OfType<Label>().Where(c => c.Tag?.ToString() == "accountDisplay").ToList())
            {
                this.Controls.Remove(control);
            }

            foreach (var panel in this.Controls.OfType<Panel>().Where(c => c.Tag?.ToString() == "accountDisplay").ToList())
            {
                this.Controls.Remove(panel);
            }

            
            AfiseazaConturiSiCarduri();
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