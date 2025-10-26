using System.Windows.Forms;

namespace KinoWinForms
{
    public partial class Form1 : Form
    {
       
        private System.ComponentModel.IContainer components = null;

        // Metoda Dispose – sprzątanie po formularzu, zwalnianie zasobów
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose(); // Zwalnianie zasobów komponentów
            }
            base.Dispose(disposing); // Wywołanie bazowej metody
        }

        // Główna metoda inicjalizująca komponenty – wywoływana w konstruktorze formularza
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container(); // Inicjalizacja kontenera komponentów
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; // Automatyczne skalowanie czcionek
            this.ClientSize = new System.Drawing.Size(800, 450); // Domyślny rozmiar okna
            this.Text = "Form1"; // Tytuł okna (zostaje nadpisany w Form1.cs)
        }
    }
}
