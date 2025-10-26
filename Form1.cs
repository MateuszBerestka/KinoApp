using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KinoWinForms
{
    public partial class Form1 : Form
    {
        private int rows = 5; 
        private int cols = 10; 
        private int filmId; 

        public Form1(int filmId)
        {
            this.filmId = filmId;

            InitializeComponent(); 
            this.FormClosed += (s, e) => Application.Exit(); 
            this.BackColor = Color.Black; 
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
z            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 600);
            this.Text = $"Rezerwacja Miejsc - Film ID: {filmId}";

            GenerateSeats(); // Generowanie przycisków miejsc
        }

        private void GenerateSeats()
        {
            int size = 50; // Rozmiar przycisków miejsc
            int margin = 10; // Odstęp między nimi
            int startX = 50; // Punkt startowy X
            int startY = 50; // Punkt startowy Y

         
            for (int col = 0; col < cols; col++)
            {
                Label seatNumber = new Label();
                seatNumber.Text = (col + 1).ToString();
                seatNumber.Size = new Size(size, size);
                seatNumber.Location = new Point(startX + col * (size + margin), startY - size);
                seatNumber.TextAlign = ContentAlignment.MiddleCenter;
                seatNumber.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                seatNumber.ForeColor = Color.White;
                this.Controls.Add(seatNumber);
            }

            // Tworzenie siatki miejsc (przyciski)
            for (int row = 0; row < rows; row++)
            {
              
                Label rowLabel = new Label();
                rowLabel.Text = ((char)('A' + row)).ToString();
                rowLabel.Size = new Size(size, size);
                rowLabel.Location = new Point(startX - size, startY + row * (size + margin));
                rowLabel.TextAlign = ContentAlignment.MiddleCenter;
                rowLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                rowLabel.ForeColor = Color.White;
                this.Controls.Add(rowLabel);

                for (int col = 0; col < cols; col++)
                {
                    Button seat = new Button();
                    seat.Size = new Size(size, size);
                    seat.Location = new Point(startX + col * (size + margin), startY + row * (size + margin));
                    seat.Tag = new Tuple<int, int>(row, col); // Zapisanie pozycji w Tagu
                    seat.BackColor = Color.LightGreen; // Dostępne miejsce = zielone

                  
                    if (DataBaseHelper.IsSeatTaken(filmId, row, col))
                    {
                        seat.BackColor = Color.Gray; // Zajęte miejsce
                        seat.Enabled = false; 
                    }

                    seat.Click += Seat_Click; // Obsługa kliknięcia
                    this.Controls.Add(seat);
                }
            }

            // Przycisk "Zarezerwuj"
            Button saveButton = new Button();
            saveButton.Text = "Zarezerwuj";
            saveButton.Size = new Size(150, 40);
            saveButton.Location = new Point(startX, startY + rows * (size + margin) + 20);
            saveButton.Click += SaveButton_Click;
            StyleButton(saveButton);
            Controls.Add(saveButton);

            // Przycisk "Powrót" – wraca do wyboru filmu
            Button backButton = new Button();
            backButton.Text = "Powrót";
            backButton.Size = new Size(150, 40);
            backButton.Location = new Point(startX + 170, startY + rows * (size + margin) + 20);
            backButton.Click += (s, e) =>
            {
                this.Hide(); // Ukrycie obecnego okna
                new MainForm().Show(); // Otwarcie formularza głównego
            };
            StyleButton(backButton);
            Controls.Add(backButton);
        }

        // Obsługa kliknięcia w miejsce – zaznaczanie i odznaczanie
        private void Seat_Click(object sender, EventArgs e)
        {
            Button seat = sender as Button;
            if (seat == null) return;

            if (seat.BackColor == Color.LightGreen) // Dostępne → wybrane
                seat.BackColor = Color.Red;
            else if (seat.BackColor == Color.Red) // Wybrane → dostępne
                seat.BackColor = Color.LightGreen;
        }

        // Zapisanie rezerwacji miejsc do bazy danych
        private void SaveButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button seat && seat.BackColor == Color.Red)
                {
                    var (row, col) = (Tuple<int, int>)seat.Tag;
                    DataBaseHelper.ReserveSeat(filmId, row, col); // Zapisz do bazy
                    seat.BackColor = Color.Gray;
                    seat.Enabled = false;
                }
            }

            MessageBox.Show("Zarezerwowano miejsca!"); // Potwierdzenie
        }

        // Styl przycisków – pomarańczowe, zaokrąglone
        private void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.Orange;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button.Width, button.Height, 10, 10));
        }

        
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
    }
}
