using Microsoft.Data.Sqlite;

namespace GangasiriTeaFactoryBilling.db
{
    public static class ConnectionBuilder
    {
        public static SqliteConnectionStringBuilder GetConnectionStringBuilder()
        {
            string dbPath = System.IO.Path.Combine(
                System.Windows.Forms.Application.StartupPath,
                "TeaFactoryDB.sqlite");

            return new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Shared,
                ForeignKeys = true,
                Pooling = true,
                DefaultTimeout = 30
            };
        }

        public static SqliteConnection GetOptimizedConnection()
        {
            var builder = GetConnectionStringBuilder();
            var connection = new SqliteConnection(builder.ConnectionString);

            // Set some pragmas for better performance
            connection.StateChange += (sender, e) =>
            {
                if (e.CurrentState == System.Data.ConnectionState.Open)
                {
                    var conn = (SqliteConnection)sender;
                    using (var cmd = conn.CreateCommand())
                    {
                        // Optimize for read-heavy operations
                        cmd.CommandText = @"
                            PRAGMA journal_mode = WAL;
                            PRAGMA synchronous = NORMAL;
                            PRAGMA cache_size = -2000;
                            PRAGMA temp_store = MEMORY;
                            PRAGMA mmap_size = 268435456;";
                        cmd.ExecuteNonQuery();
                    }
                }
            };

            return connection;
        }
    }
}