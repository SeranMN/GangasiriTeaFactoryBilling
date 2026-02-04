using GangasiriTeaFactoryBilling.db;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.db
{
    public static class DatabaseBackup
    {
        public static void CreateBackup()
        {
            try
            {
                string sourcePath = Path.Combine(Application.StartupPath, "TeaFactoryDB.sqlite");
                string backupDir = Path.Combine(Application.StartupPath, "Backups");

                // Create backup directory if it doesn't exist
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupPath = Path.Combine(backupDir, $"TeaFactoryDB_Backup_{timestamp}.sqlite");

                // Simple file copy backup
                File.Copy(sourcePath, backupPath, true);

                // Also create a vacuum backup for better performance
                string vacuumBackupPath = Path.Combine(backupDir, $"TeaFactoryDB_Vacuum_{timestamp}.sqlite");
                using (var conn = new SqliteConnection($"Data Source={sourcePath}"))
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand($"VACUUM INTO '{vacuumBackupPath}'", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Backup created successfully!\n\nRegular backup: {backupPath}\nOptimized backup: {vacuumBackupPath}",
                    "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Backup failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RestoreBackup(string backupFilePath)
        {
            try
            {
                var result = MessageBox.Show(
                    "WARNING: This will replace your current database with the backup.\n" +
                    "Make sure you have a current backup before proceeding.\n\n" +
                    "Do you want to continue?",
                    "Confirm Database Restore",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                string dbPath = Path.Combine(Application.StartupPath, "TeaFactoryDB.sqlite");

                // Backup current database first
                string currentBackup = Path.Combine(Application.StartupPath,
                    $"TeaFactoryDB_BeforeRestore_{DateTime.Now:yyyyMMdd_HHmmss}.sqlite");
                File.Copy(dbPath, currentBackup, true);

                // Restore from backup
                File.Copy(backupFilePath, dbPath, true);

                MessageBox.Show($"Database restored successfully!\n\nCurrent database backed up to: {currentBackup}",
                    "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Restore failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CompactDatabase()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("VACUUM", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Database compacted successfully!\n\nThis optimizes database performance and reduces file size.",
                    "Compaction Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Compaction failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string GetDatabaseInfo()
        {
            try
            {
                string dbPath = Path.Combine(Application.StartupPath, "TeaFactoryDB.sqlite");

                if (!File.Exists(dbPath))
                    return "Database file not found.";

                var fileInfo = new FileInfo(dbPath);
                long size = fileInfo.Length;

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Get table counts
                    var tables = new[]
                    {
                        "Suppliers", "Lines", "TeaRates", "DailyCollection", "Advances", "MonthlyInvoices"
                    };

                    string info = $"Database: {dbPath}\n";
                    info += $"Size: {FormatFileSize(size)}\n";
                    info += $"Created: {fileInfo.CreationTime:yyyy-MM-dd HH:mm}\n";
                    info += $"Modified: {fileInfo.LastWriteTime:yyyy-MM-dd HH:mm}\n\n";
                    info += "Record Counts:\n";

                    foreach (var table in tables)
                    {
                        using (var cmd = new SqliteCommand($"SELECT COUNT(*) FROM {table}", conn))
                        {
                            var count = cmd.ExecuteScalar();
                            info += $"  {table}: {count}\n";
                        }
                    }

                    return info;
                }
            }
            catch (Exception ex)
            {
                return $"Error getting database info: {ex.Message}";
            }
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double len = bytes;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}