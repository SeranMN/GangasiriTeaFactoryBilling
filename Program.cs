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
            string regularFontPath = Path.Combine(appBaseDirectory, "Fonts/static", "NotoSansSinhala-Regular.ttf");
            string boldFontPath = Path.Combine(appBaseDirectory, "Fonts/static", "NotoSansSinhala-Bold.ttf");
            if (File.Exists(regularFontPath))
            {
                FontManager.RegisterFont(File.OpenRead(regularFontPath));
            }
            else
            {
                MessageBox.Show($"Font not found at: {regularFontPath}", "Font Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (File.Exists(boldFontPath))
            {
                FontManager.RegisterFont(File.OpenRead(boldFontPath));
            }
            else
            {
                MessageBox.Show($"Font not found at: {boldFontPath}", "Font Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}