using System;
using System.Drawing;
using System.Windows.Forms;

namespace KinoWinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

           
            this.BackColor = Color.Black;

            // Wyśrodkowanie okna na ekranie
            this.StartPosition = FormStartPosition.CenterScreen;

            // Ustawienie tytułu okna
            this.Text = "Wybór Filmu";

            // Brak możliwości zmiany rozmiaru okna
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Rozmiar okna
            this.Size = new Size(1000, 600);

            // Zamyka całą aplikację przy zamknięciu okna
            this.FormClosed += (s, e) => Application.Exit();

            // Załaduj filmy z bazy danych i wyświetl je
            LoadMovies();
        }

        private void LoadMovies()
        {
          
            var movies = DataBaseHelper.GetMovies();

            int margin = 30;     
            int width = 150;      
            int height = 250;    
            int x = margin;        
            int y = 50;            

            foreach (var movie in movies)
            {
                
                var panel = new Panel();
                panel.Size = new Size(width, height + 70); 
                panel.Location = new Point(x, y);
                panel.BackColor = Color.Black;

                var pictureBox = new PictureBox();
                pictureBox.Size = new Size(width, height);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                using (var img = Image.FromFile(movie.ImagePath))
                {
                    pictureBox.Image = (Image)img.Clone(); // Klonowanie, żeby nie trzymać otwartego pliku
                }
                panel.Controls.Add(pictureBox); // Dodaj obrazek do panelu

                // Tworzenie etykiety z tytułem filmu
                var label = new Label();
                label.Text = movie.Title;
                label.ForeColor = Color.White;
                label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Dock = DockStyle.Bottom;
                label.Height = 40;
                panel.Controls.Add(label); // Dodaj tytuł do panelu

                
                // Funkcja otwierająca salę kinową z miejscami
                void OpenRoom()
                {
                    this.Hide(); // Ukryj główne okno
                    new Form1(movie.Id).Show(); // Pokaż formularz z salą
                }

                // Kliknięcie w plakat lub tytuł też otwiera salę
                pictureBox.Click += (s, e) => OpenRoom();
                label.Click += (s, e) => OpenRoom();

                // Dodanie całego panelu do formularza
                this.Controls.Add(panel);

                // Przesuwamy X do następnego filmu
                x += width + margin;

                // Jeśli nie mieści się w szerokości okna – przejdź do nowego wiersza
                if (x + width > this.Width - margin)
                {
                    x = margin;
                    y += height + 100;
                }
            }
        }

        // Import biblioteki do zaokrąglania rogów przycisków
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
    }
}
