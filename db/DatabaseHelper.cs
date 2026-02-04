using System;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Windows.Forms;
using GangasiriTeaFactoryBilling.Audit;

namespace GangasiriTeaFactoryBilling.db
{
    public static class DatabaseHelper
    {
        private static string databasePath = Path.Combine(Application.StartupPath, "TeaFactoryDB.sqlite");
        private static string connectionString = $"Data Source={databasePath}";
        public static string ConnectionString => connectionString;

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }

        public static void InitializeDatabase()
        {
            try
            {
                // Check if database exists, create if not
                bool isNewDatabase = !File.Exists(databasePath);

                using (var conn = GetConnection())
                {
                    conn.Open();

                    // Enable foreign keys
                    using (var cmd = new SqliteCommand("PRAGMA foreign_keys = ON;", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Create tables
                    CreateTables(conn);

                    // Ensure schema updates (migrations)
                    EnsureSchemaUpdates(conn);

                    // Ensure default user exists
                    EnsureDefaultUser(conn);

                    // Insert sample data if new database
                    if (isNewDatabase)
                    {
                        //InsertSampleData(conn);
                        MessageBox.Show("Database created successfully with sample data!", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    conn.Close();
                }

                Console.WriteLine("Database initialized successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void EnsureSchemaUpdates(SqliteConnection conn)
        {
            try
            {
                // Check if DailyCollection has Status column
                bool hasStatus = false;
                using (var cmd = new SqliteCommand("PRAGMA table_info(DailyCollection);", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == "Status")
                        {
                            hasStatus = true;
                            break;
                        }
                    }
                }

                if (!hasStatus)
                {
                    using (var cmd = new SqliteCommand("ALTER TABLE DailyCollection ADD COLUMN Status TEXT DEFAULT 'Active';", conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Added Status column to DailyCollection table.");
                    }
                }

                // Check if Advances has Status column
                bool hasAdvanceStatus = false;
                using (var cmd = new SqliteCommand("PRAGMA table_info(Advances);", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == "Status")
                        {
                            hasAdvanceStatus = true;
                            break;
                        }
                    }
                }

                if (!hasAdvanceStatus)
                {
                    using (var cmd = new SqliteCommand("ALTER TABLE Advances ADD COLUMN Status TEXT DEFAULT 'Active';", conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Added Status column to Advances table.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Schema update failed: {ex.Message}");
            }
        }

        private static void CreateTables(SqliteConnection conn)
        {
            // Lines Table
            string createLinesTable = @"
                CREATE TABLE IF NOT EXISTS Lines (
                    LineID INTEGER PRIMARY KEY AUTOINCREMENT,
                    LineName TEXT NOT NULL,
                    Description TEXT,
                    TransportFee REAL DEFAULT 0,
                    Status TEXT DEFAULT 'Active',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            // Suppliers Table
            string createSuppliersTable = @"
                CREATE TABLE IF NOT EXISTS Suppliers (
                    SupplierID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierNumber TEXT UNIQUE NOT NULL,
                    SupplierName TEXT NOT NULL,
                    Telephone TEXT,
                    LineID INTEGER,
                    Address TEXT,
                    DueAmount REAL DEFAULT 0,
                    Status TEXT DEFAULT 'Active',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (LineID) REFERENCES Lines(LineID) ON DELETE SET NULL
                );";

            // Tea Rates Table
            string createTeaRatesTable = @"
                CREATE TABLE IF NOT EXISTS TeaRates (
                    RateID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Rate REAL NOT NULL,
                    ValidFrom DATE NOT NULL,
                    ValidTo DATE NOT NULL,
                    Status TEXT DEFAULT 'Active',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            // Daily Collection Table
            string createDailyCollectionTable = @"
                CREATE TABLE IF NOT EXISTS DailyCollection (
                    CollectionID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierID INTEGER NOT NULL,
                    CollectionDate DATE NOT NULL,
                    Weight REAL NOT NULL,
                    RateID INTEGER,
                    IsTransportAdd INTEGER DEFAULT 0,
                    TotalAmount REAL NOT NULL,
                    Notes TEXT,
                    Status TEXT DEFAULT 'Active',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE CASCADE,
                    FOREIGN KEY (RateID) REFERENCES TeaRates(RateID) ON DELETE SET NULL
                );";

            // Advances Table
            string createAdvancesTable = @"
                CREATE TABLE IF NOT EXISTS Advances (
                    AdvanceID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierID INTEGER NOT NULL,
                    AdvanceDate DATE NOT NULL,
                    Amount REAL NOT NULL,
                    Description TEXT,
                    Status TEXT DEFAULT 'Active',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE CASCADE
                );";


            // Monthly Invoices Table
            string createInvoicesTable = @"
                CREATE TABLE IF NOT EXISTS MonthlyInvoices (
                    InvoiceID INTEGER PRIMARY KEY AUTOINCREMENT,
                    SupplierID INTEGER NOT NULL,
                    InvoiceMonth TEXT NOT NULL,
                    Year INTEGER,
                    TotalWeight REAL,
                    RatePerKg REAL,
                    TransportAllowance REAL,
                    TransportFee REAL,
                    TotalAmount REAL,
                    TotalAdvances REAL,
                    TotalDeductions REAL,
                    PreviousBalance REAL,
                    NetAmount REAL,
                    Status TEXT DEFAULT 'Pending',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID) ON DELETE CASCADE
                );";

            //Tea Pcket Sell

            string createTeaPacketTable = @"CREATE TABLE IF NOT EXISTS TeaPacketSell (
	                                        SellId	INTEGER,
	                                        SupplierID	INTEGER,
	                                        Price	REAL,
	                                        Qty	INTEGER,
	                                        Total	REAL,
	                                        Date	TEXT,
	                                        PRIMARY KEY(SellId AUTOINCREMENT),
	                                        CONSTRAINT ""FK"" FOREIGN KEY(SupplierID) REFERENCES Suppliers(SupplierID)
                                            );";

            // Users, System Settings, and Audit Logs Tables
            string createUsersAndSettingsAndAuditTables = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL DEFAULT 'User',
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                );
                
                CREATE TABLE IF NOT EXISTS SystemSettings (
                    SettingKey TEXT PRIMARY KEY,
                    SettingValue TEXT,
                    LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                CREATE TABLE IF NOT EXISTS AuditLogs (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER,
                    Username TEXT,
                    ActionType TEXT,
                    TableName TEXT,
                    RecordID TEXT,
                    Details TEXT,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            // Execute table creation
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = createLinesTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createSuppliersTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createTeaRatesTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createDailyCollectionTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createAdvancesTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createInvoicesTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createTeaPacketTable;
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = createUsersAndSettingsAndAuditTables;
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static void InsertSampleData(SqliteConnection conn)
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        // Insert sample lines
                        cmd.CommandText = @"
                            INSERT INTO Lines (LineName, Description, TransportFee, Status) VALUES
                            ('Line 1', 'Main collection line for northern area', 150.00, 'Active'),
                            ('Line 2', 'Secondary line for southern region', 200.00, 'Active'),
                            ('Line 3', 'Premium line for high-quality leaves', 250.00, 'Active');";
                        cmd.ExecuteNonQuery();

                        // Insert sample suppliers
                        cmd.CommandText = @"
                            INSERT INTO Suppliers (SupplierNumber, SupplierName, Telephone, LineID, Address, Status) VALUES
                            ('S-001', 'Kamal Perera', '077-1234567', 1, '123 Main Street, Colombo', 'Active'),
                            ('S-002', 'Sunil Fernando', '071-2345678', 2, '456 Galle Road, Galle', 'Active'),
                            ('S-003', 'Anura Silva', '072-3456789', 3, '789 Kandy Road, Kandy', 'Active'),
                            ('S-004', 'Nimal Rathnayake', '076-4567890', 1, '321 Negombo Road, Negombo', 'Active'),
                            ('S-005', 'Sampath Bandara', '075-5678901', 2, '654 Matara Road, Matara', 'Active');";
                        cmd.ExecuteNonQuery();

                        // Insert sample tea rates
                        cmd.CommandText = @"
                            INSERT INTO TeaRates (Rate, ValidFrom, ValidTo, Status) VALUES
                            (85.00, date('now', '-1 month'), date('now'), 'Active'),
                            (82.50, date('now', '-2 months'), date('now', '-1 month'), 'Expired'),
                            (87.50, date('now'), date('now', '+1 month'), 'Active');";
                        cmd.ExecuteNonQuery();

                        // Insert sample daily collections
                        cmd.CommandText = @"
                            INSERT INTO DailyCollection (SupplierID, CollectionDate, Weight, Rate, TransportFee, TotalAmount, Notes) VALUES
                            (1, date('now'), 125.50, 85.00, 150.00, 10817.50, 'Morning collection'),
                            (2, date('now'), 98.75, 85.00, 0, 8393.75, ''),
                            (4, date('now'), 156.25, 85.00, 200.00, 13481.25, 'With transport');";
                        cmd.ExecuteNonQuery();

                        // Insert sample advances
                        cmd.CommandText = @"
                            INSERT INTO Advances (SupplierID, AdvanceDate, Amount, Description, Status) VALUES
                            (1, date('now'), 5000.00, 'Monthly advance', 'Active'),
                            (2, date('now', '-1 day'), 3000.00, 'Emergency advance', 'Active'),
                            (4, date('now', '-5 days'), 7500.00, 'Festival advance', 'Deducted');";
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static void EnsureDefaultUser(SqliteConnection conn)
        {
            try 
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Users";
                    long count = (long)cmd.ExecuteScalar();
                    
                    if (count == 0)
                    {
                        cmd.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
                        cmd.Parameters.AddWithValue("@username", "admin");
                        cmd.Parameters.AddWithValue("@password", "admin123");
                        cmd.Parameters.AddWithValue("@role", "Admin");
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Default admin user created.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle quietly if table structure matches
                Console.WriteLine("Error ensuring default user: " + ex.Message);
            }
        }

        public static dynamic GetUser(string username)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("SELECT UserID, Username, Role FROM Users WHERE Username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    Username = reader["Username"].ToString(),
                                    Role = reader["Role"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching user: " + ex.Message);
            }
            return null;
        }

        public static bool AddUser(string username, string password, string role)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.ExecuteNonQuery();
                        
                        AuditManager.LogAction("Create", "Users", username, $"User '{username}' created with role '{role}'");
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool UpdatePassword(int userId, string newPassword)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("UPDATE Users SET Password = @password WHERE UserID = @userId", conn))
                    {
                        cmd.Parameters.AddWithValue("@password", newPassword);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                        
                        AuditManager.LogAction("Update", "Users", userId.ToString(), "Password updated");
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool CheckPassword(int userId, string password)
        {
             try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE UserID = @userId AND Password = @password", conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@password", password);
                        return (long)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public static System.Data.DataTable GetUsers()
        {
            try
            {
                var dt = new System.Data.DataTable();
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("SELECT UserID, Username, Role, CreatedDate FROM Users", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch 
            {
                return null;
            }
        }

        public static bool CheckUser(string username, string password)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password", conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        
                        long count = (long)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Authentication error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("SELECT 1", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void BackupDatabase()
        {
            try
            {
                string customPath = GetSetting("BackupFolderPath");
                string backupDir = !string.IsNullOrEmpty(customPath) && Directory.Exists(customPath) 
                    ? customPath 
                    : Path.Combine(Application.StartupPath, "Backups");

                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                string backupPath = Path.Combine(backupDir,
                    $"TeaFactoryDB_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.sqlite");

                File.Copy(databasePath, backupPath, true);

                // Also backup using VACUUM INTO for a clean backup
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand($"VACUUM INTO '{backupPath.Replace(".sqlite", "_vacuum.sqlite")}'", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"Database backed up to: {backupPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup failed: {ex.Message}");
            }
        }

        public static void RestoreDatabase(string backupPath)
        {
            try
            {
                // Ensure connections are closed
                SqliteConnection.ClearAllPools();
                
                File.Copy(backupPath, databasePath, true);
                
                MessageBox.Show("Database restored successfully! The application will now restart.", "Restore Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Restore failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void SaveSetting(string key, string value)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string sql = @"INSERT INTO SystemSettings (SettingKey, SettingValue, LastUpdated) 
                                 VALUES (@key, @value, CURRENT_TIMESTAMP)
                                 ON CONFLICT(SettingKey) DO UPDATE SET SettingValue = @value, LastUpdated = CURRENT_TIMESTAMP;";
                                 
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@key", key);
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving setting {key}: {ex.Message}");
            }
        }

        public static string GetSetting(string key)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("SELECT SettingValue FROM SystemSettings WHERE SettingKey = @key", conn))
                    {
                        cmd.Parameters.AddWithValue("@key", key);
                        var result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value ? result.ToString() : null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static void CompactDatabase()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand("VACUUM", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Database compaction failed: {ex.Message}", ex);
            }
        }

        public static long GetDatabaseSize()
        {
            if (File.Exists(databasePath))
            {
                var fileInfo = new FileInfo(databasePath);
                return fileInfo.Length;
            }
            return 0;
        }

        public static int ExecuteNonQuery(string sql, params SqliteParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, params SqliteParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
        public static void PerformAutoBackup()
        {
            try
            {
                string autoInfo = GetSetting("AutoBackup");
                if (autoInfo != "True") return;

                string freq = GetSetting("BackupFrequency") ?? "Daily";
                string lastBackupStr = GetSetting("LastBackupDate");
                
                DateTime lastBackup = DateTime.MinValue;
                if (!string.IsNullOrEmpty(lastBackupStr))
                    DateTime.TryParse(lastBackupStr, out lastBackup);

                bool shouldBackup = false;
                if (freq == "Daily" && (DateTime.Now - lastBackup).TotalDays >= 1) shouldBackup = true;
                else if (freq == "Weekly" && (DateTime.Now - lastBackup).TotalDays >= 7) shouldBackup = true;
                else if (freq == "Monthly" && (DateTime.Now - lastBackup).TotalDays >= 30) shouldBackup = true;

                if (shouldBackup)
                {
                    BackupDatabase();
                    SaveSetting("LastBackupDate", DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Auto backup failed: " + ex.Message);
            }
        }
    }
}