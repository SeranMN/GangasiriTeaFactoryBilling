using System;

namespace GangasiriTeaFactoryBilling.Audit
{
    public class AuditLog
    {
        public int LogID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string ActionType { get; set; } // e.g., INSERT, UPDATE, DELETE
        public string TableName { get; set; }
        public string RecordID { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
