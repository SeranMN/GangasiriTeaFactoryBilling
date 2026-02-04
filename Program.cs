using GangasiriTeaFactoryBilling.db;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Windows.Forms;
using QuestPDF.Fluent;
using QuestPDF.Drawing;
namespace GangasiriTeaFactoryBilling
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // Initialize application
            InitializeApplication();
            InitializeQuestPDF();
            // Run main form
            Application.Run(new frmLogin());
        }
        static void InitializeApplication()
        {
            try
            {
                // Show splash screen or loading message
                Application.DoEvents();
                // Initialize database
                DatabaseHelper.InitializeDatabase();
                DatabaseHelper.PerformAutoBackup();
                
                // Check for updates asynchronously
                _ = Updater.GitUpdateManager.CheckForUpdates();
                // Test connection
                if (DatabaseHelper.TestConnection())
                {
                    Console.WriteLine("✓ Database connected successfully!");
                    // Show database info in console
                    var dbInfo = DatabaseBackup.GetDatabaseInfo();
                    Console.WriteLine("\n" + dbInfo);
                }
                else
                {
                    MessageBox.Show("Failed to connect to database. Application may not function properly.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Application initialization failed: {ex.Message}\n\nPlease check if SQLite is properly installed.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        static void InitializeQuestPDF()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            string appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fontPath = Path.Combine(appBaseDirectory, "Fonts", "NotoSansSinhala-VariableFont_wdth,wght.ttf");

            if (File.Exists(fontPath))
            {
                // Register the font
                using (var stream = File.OpenRead(fontPath))
                {
                    FontManager.RegisterFont(stream);
                }
            }
            else
            {
                // Fallback check for other common names or locations if needed, or just error
                MessageBox.Show($"Font not found at: {fontPath}", "Font Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}