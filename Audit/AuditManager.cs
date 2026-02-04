using System;
using GangasiriTeaFactoryBilling.db;
using Microsoft.Data.Sqlite;

namespace GangasiriTeaFactoryBilling.Audit
{
    public static class AuditManager
    {
        public static void LogAction(string actionType, string tableName, string recordId, string details)
        {
            try
            {
                // Check if global audit is enabled for this table (Default to true if not set)
                if (!IsAuditEnabled(tableName)) return;

                int userId = Session.UserID;
                string username = Session.Username ?? "System";

                string query = @"
                    INSERT INTO AuditLogs (UserID, Username, ActionType, TableName, RecordID, Details, Timestamp) 
                    VALUES (@UserID, @Username, @ActionType, @TableName, @RecordID, @Details, @Timestamp)";

                using (var connection = new SqliteConnection(DatabaseHelper.ConnectionString))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@ActionType", actionType);
                        command.Parameters.AddWithValue("@TableName", tableName);
                        command.Parameters.AddWithValue("@RecordID", recordId ?? "");
                        command.Parameters.AddWithValue("@Details", details ?? "");
                        command.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently fail logging to not disrupt main flow, but ideally log to file
                Console.WriteLine($"Audit Log Fail: {ex.Message}");
            }
        }

        public static bool IsAuditEnabled(string tableName)
        {
            // Default to true. Only return false if explicitly set to "False"
            string setting = DatabaseHelper.GetSetting($"AuditEnabled_{tableName}");
            return setting != "False";
        }
        
        public static void SetAuditEnabled(string tableName, bool enabled)
        {
            DatabaseHelper.SaveSetting($"AuditEnabled_{tableName}", enabled.ToString());
        }
    }
}
