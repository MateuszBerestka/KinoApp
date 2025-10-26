using System;
using System.Windows.Forms;

namespace KinoWinForms
{
    internal static class Program
    {
        
        [STAThread]
        static void Main()
        {
           
            ApplicationConfiguration.Initialize();

            try
            {
              
                var mainForm = new MainForm();

                
                mainForm.FormClosed += (s, e) => Application.Exit();

               
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
               
                MessageBox.Show(
                    "Błąd podczas uruchamiania aplikacji:\n\n" + ex.Message,
                    "Błąd Krytyczny",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
