using Dapper;
using GangasiriTeaFactoryBilling.Models;
using Microsoft.Data.Sqlite;
using GangasiriTeaFactoryBilling.Audit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GangasiriTeaFactoryBilling.db
{
    
    public static class DataAccess
    {
        #region LINES
        // ========== LINES ==========
        public static List<Line> GetAllLines()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                return conn.Query<Line>("SELECT * FROM Lines ORDER BY LineName").ToList();
            }
        }

        public static Line GetLineById(int lineId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                return conn.QueryFirstOrDefault<Line>(
                    "SELECT * FROM Lines WHERE LineID = @LineID",
                    new { LineID = lineId });
            }
        }

        public static int AddLine(Line line)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO Lines (LineID,LineName, Description, TransportFee, Status)
                    VALUES (@LineID,@LineName, @Description, @TransportFee, @Status);
                    SELECT last_insert_rowid();";

                int newId = conn.ExecuteScalar<int>(sql, line);
                AuditManager.LogAction("Create", "Lines", newId.ToString(), $"Added Line: {line.LineName}");
                return newId;
            }
        }

        public static bool UpdateLine(Line line)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE Lines 
                    SET LineName = @LineName, 
                        Description = @Description, 
                        TransportFee = @TransportFee,
                        Status = @Status
                    WHERE LineID = @LineID";

                bool success = conn.Execute(sql, line) > 0;
                if (success) AuditManager.LogAction("Update", "Lines", line.LineID.ToString(), $"Updated Line: {line.LineName}");
                return success;
            }
        }

        public static bool DeleteLine(string lineId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                bool success = conn.Execute("DELETE FROM Lines WHERE LineID = @LineID",
                    new { LineID = lineId }) > 0;
                if(success) AuditManager.LogAction("Delete", "Lines", lineId, "Deleted Line");
                return success;
            }
        }

#endregion
        #region SUPPLIERS
        // ========== SUPPLIERS ==========
        public static List<Supplier> GetAllSuppliers()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT s.*, l.LineName 
                    FROM Suppliers s
                    LEFT JOIN Lines l ON s.LineID = l.LineID
                    ORDER BY s.SupplierName";

                return conn.Query<Supplier>(sql).ToList();
            }
        }

        public static List<Supplier> GetActiveSuppliers()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT s.*, l.LineName 
                    FROM Suppliers s
                    LEFT JOIN Lines l ON s.LineID = l.LineID
                    WHERE s.Status = 'Active' OR s.Status IS NULL
                    ORDER BY s.SupplierName";

                return conn.Query<Supplier>(sql).ToList();
            }
        }

        public static Supplier GetSupplierById(string supplierId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT s.*, l.LineName , l.TransportFee
                    FROM Suppliers s
                    LEFT JOIN Lines l ON s.LineID = l.LineID
                    WHERE s.SupplierID = @SupplierID";

                return conn.QueryFirstOrDefault<Supplier>(sql, new { SupplierID = supplierId });
            }
        }

        public static Supplier GetSupplierByNumber(string supplierNumber)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT s.*, l.LineName 
                    FROM Suppliers s
                    LEFT JOIN Lines l ON s.LineID = l.LineID
                    WHERE s.SupplierNumber = @SupplierNumber";

                return conn.QueryFirstOrDefault<Supplier>(sql, new { SupplierNumber = supplierNumber });
            }
        }

        public static int AddSupplier(Supplier supplier)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO Suppliers (SupplierNumber, SupplierName, Telephone, LineID, Address, DueAmount, Status)
                    VALUES (@SupplierNumber, @SupplierName, @Telephone, @LineID, @Address, @DueAmount, @Status);
                    SELECT last_insert_rowid();";

                int newId = conn.ExecuteScalar<int>(sql, supplier);
                
                AuditManager.LogAction("Create", "Suppliers", supplier.SupplierNumber, $"Supplier '{supplier.SupplierName}' added");
                
                return newId;
            }
        }

        public static bool UpdateSupplier(Supplier supplier)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE Suppliers 
                    SET SupplierNumber = @SupplierNumber,
                        SupplierName = @SupplierName,
                        Telephone = @Telephone,
                        LineID = @LineID,
                        Address = @Address,
                        Status = @Status
                    WHERE SupplierID = @SupplierID";

                bool success = conn.Execute(sql, supplier) > 0;
                
                if (success)
                {
                    AuditManager.LogAction("Update", "Suppliers", supplier.SupplierNumber, $"Supplier '{supplier.SupplierName}' updated");
                }
                
                return success;
            }
        }

        public static bool DeleteSupplier(int supplierId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                bool success = conn.Execute("UPDATE Suppliers SET Status = 'Inactive' WHERE SupplierID = @SupplierID",
                    new { SupplierID = supplierId }) > 0;
                
                if (success)
                {
                    // Get supplier number for logging (optional, could query before update)
                    AuditManager.LogAction("Deactivate", "Suppliers", supplierId.ToString(), "Soft Deleted (Deactivated) Supplier");
                }
                return success;
            }
        }

        public static bool ActivateSupplier(int supplierId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                bool success = conn.Execute("UPDATE Suppliers SET Status = 'Active' WHERE SupplierID = @SupplierID",
                    new { SupplierID = supplierId }) > 0;

                if (success)
                {
                    AuditManager.LogAction("Activate", "Suppliers", supplierId.ToString(), "Activated Supplier");
                }
                return success;
            }
        }

        public static bool UpdateSupplierDueAmount(int supplierId, decimal dueAmount)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                bool success = conn.Execute("UPDATE Suppliers SET DueAmount = @DueAmount WHERE SupplierID = @SupplierID",
                    new { SupplierID = supplierId, DueAmount = dueAmount }) > 0;
                return success;
            }
        }
#endregion
        #region TEA RATES
        // ========== TEA RATES ==========
        public static List<TeaRate> GetAllTeaRates()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                return conn.Query<TeaRate>("SELECT * FROM TeaRates ORDER BY ValidFrom DESC").ToList();
            }
        }

        public static TeaRate GetCurrentTeaRate()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT * FROM TeaRates 
                    WHERE date('now') BETWEEN ValidFrom AND ValidTo
                    ORDER BY ValidFrom DESC
                    LIMIT 1";

                return conn.QueryFirstOrDefault<TeaRate>(sql);
            }
        }

        public static TeaRate GetTeaRateByDate(DateTime date)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT * FROM TeaRates 
                    WHERE date(@Date) BETWEEN date(ValidFrom) AND date(ValidTo)
                    ORDER BY ValidFrom DESC
                    LIMIT 1";

                return conn.QueryFirstOrDefault<TeaRate>(sql, new { Date = date.ToString("yyyy-MM-dd") });
            }
        }

        public static TeaRate GetTeaRateByMonth(int year, int month)
        {
            // Construct a DateTime object for the first day of the specified month and year
            DateTime date = new DateTime(year, month, 1);
            // Call the existing GetTeaRateByDate method
            return GetTeaRateByDate(date);
        }

        public static int AddTeaRate(TeaRate rate)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO TeaRates (Rate, ValidFrom, ValidTo, Status)
                    VALUES (@Rate, @ValidFrom, @ValidTo, @Status);
                    SELECT last_insert_rowid();";

                int newId = conn.ExecuteScalar<int>(sql, rate);
                AuditManager.LogAction("Create", "TeaRates", newId.ToString(), $"Added Rate: {rate.Rate}");
                return newId;
            }
        }

        public static bool UpdateTeaRate(TeaRate rate)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE TeaRates 
                    SET Rate = @Rate,
                        ValidFrom = @ValidFrom,
                        ValidTo = @ValidTo,
                        Status = @Status
                    WHERE RateID = @RateID";

                bool success = conn.Execute(sql, rate) > 0;
                if(success) AuditManager.LogAction("Update", "TeaRates", rate.RateID.ToString(), $"Updated Rate to {rate.Rate}");
                return success;
            }
        }
#endregion
        #region DAILY COLLECTION
        // ========== DAILY COLLECTION ==========
        public static List<DailyCollection> GetDailyCollections(DateTime? date = null)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT dc.*, s.SupplierName
                    FROM DailyCollection dc
                    JOIN Suppliers s ON dc.SupplierID = s.SupplierID
                    WHERE (dc.Status != 'Inactive' OR dc.Status IS NULL)";
                   
                if (date.HasValue)
                {
                    sql += " AND date(dc.CollectionDate) = date(@Date)";
                }

                sql += " ORDER BY dc.CreatedDate DESC";

                return conn.Query<DailyCollection>(sql, new { Date = date?.ToString("yyyy-MM-dd") }).ToList();
            }
        }

        public static List<DailyCollection> GetSupplierCollections(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT dc.*, s.SupplierName 
                    FROM DailyCollection dc
                    JOIN Suppliers s ON dc.SupplierID = s.SupplierID
                    WHERE dc.SupplierID = @SupplierID AND (dc.Status != 'Inactive' OR dc.Status IS NULL)
                   ";
                if (startDate.HasValue)
                {
                    sql += " AND dc.CollectionDate >= @StartDate";
                }
                if (endDate.HasValue)
                {
                    sql += " AND dc.CollectionDate <= @EndDate";
                }

                sql += " ORDER BY dc.CollectionDate";

                return conn.Query<DailyCollection>(sql, new
                {
                    SupplierID = supplierId,
                    StartDate = startDate?.ToString("yyyy-MM-dd"),
                    EndDate = endDate?.ToString("yyyy-MM-dd")
                }).ToList();
            }

           
        }

        public static decimal GetSupplierTotalWeightByMonth(int supplierId, int year, int month)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT COALESCE(SUM(Weight), 0) as TotalWeight
            FROM DailyCollection
            WHERE SupplierID = @SupplierID
           
            AND CAST(strftime('%Y', CollectionDate) as INTEGER) = @Year
            AND CAST(strftime('%m', CollectionDate) as INTEGER) = @Month
            AND (Status != 'Inactive' OR Status IS NULL)";

                return conn.QuerySingleOrDefault<decimal>(sql, new
                {
                    SupplierID = supplierId,
                    Year = year,
                    Month = month
                });
            }
        }
        public static List<DailyCollection> GetSupplierWeightByMonth(int supplierId, int year, int month)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"Select *
            FROM DailyCollection
            WHERE SupplierID = @SupplierID
            AND CAST(strftime('%Y', CollectionDate) as INTEGER) = @Year
            AND CAST(strftime('%m', CollectionDate) as INTEGER) = @Month
            AND (Status != 'Inactive' OR Status IS NULL)";

                return conn.Query<DailyCollection>(sql, new
                {
                    SupplierID = supplierId,
                    Year = year,
                    Month = month
                }).ToList();
            }
        }

        public static decimal GetSupplierTotalWeightWithTransportByMonth(int supplierId, int year, int month)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT COALESCE(SUM(Weight), 0) as TotalWeight
            FROM DailyCollection
            WHERE SupplierID = @SupplierID
            AND IsTransportAdd = 'Yes'
            AND CAST(strftime('%Y', CollectionDate) as INTEGER) = @Year
            AND CAST(strftime('%m', CollectionDate) as INTEGER) = @Month
            AND (Status != 'Inactive' OR Status IS NULL)";

                return conn.QuerySingleOrDefault<decimal>(sql, new
                {
                    SupplierID = supplierId,
                    Year = year,
                    Month = month
                });
            }
        }
        public static decimal GetSupplierTotalWeightWithoutTransportByMonth(int supplierId, int year, int month)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT COALESCE(SUM(Weight), 0) as TotalWeight
            FROM DailyCollection
            WHERE SupplierID = @SupplierID
            AND IsTransportAdd = 'No'
            AND CAST(strftime('%Y', CollectionDate) as INTEGER) = @Year
            AND CAST(strftime('%m', CollectionDate) as INTEGER) = @Month
            AND (Status != 'Inactive' OR Status IS NULL)";

                return conn.QuerySingleOrDefault<decimal>(sql, new
                {
                    SupplierID = supplierId,
                    Year = year,
                    Month = month
                });
            }
        }


        public static int AddDailyCollection(DailyCollection collection)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO DailyCollection (SupplierID, CollectionDate, Weight, IsTransportAdd, TotalAmount, Notes)
                    VALUES (@SupplierID, @CollectionDate, @Weight, @IsTransportAdd, @TotalAmount, @Notes);
                    SELECT last_insert_rowid();";

                int newId = conn.ExecuteScalar<int>(sql, collection);
                AuditManager.LogAction("Create", "DailyCollection", newId.ToString(), $"Added Collection for Supplier {collection.SupplierID}, Weight: {collection.Weight}");
                return newId;
            }
        }

        public static bool UpdateDailyCollection(DailyCollection collection)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE DailyCollection 
                    SET SupplierID = @SupplierID,
                        CollectionDate = @CollectionDate,
                        Weight = @Weight,
                        IsTransportAdd = @IsTransportAdd,
                        TotalAmount = @TotalAmount,
                        Notes = @Notes
                    WHERE CollectionID = @CollectionID";

                bool success = conn.Execute(sql, collection) > 0;
                if(success) AuditManager.LogAction("Update", "DailyCollection", collection.CollectionID.ToString(), $"Updated Collection ID {collection.CollectionID}");
                return success;
            }
        }

        public static bool DeleteDailyCollection(string collectionId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                bool success = conn.Execute("UPDATE DailyCollection SET Status ='Inactive' WHERE CollectionID = @CollectionID",
                    new { CollectionID = collectionId }) > 0;
                if(success) AuditManager.LogAction("Delete", "DailyCollection", collectionId, "Soft Deleted Collection");
                return success;
            }
        }
        #endregion
        #region ADVANCES
        // ========== ADVANCES ==========
        public static List<Advance> GetAllAdvances()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT a.*, s.SupplierName 
                    FROM Advances a
                    JOIN Suppliers s ON a.SupplierID = s.SupplierID
                    WHERE (a.Status != 'Inactive' OR a.Status IS NULL)
                    ORDER BY a.AdvanceDate DESC";

                return conn.Query<Advance>(sql).ToList();
            }
        }

        public static List<Advance> GetSupplierAdvances(int supplierId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT a.*, s.SupplierName 
                    FROM Advances a
                    JOIN Suppliers s ON a.SupplierID = s.SupplierID
                    WHERE a.SupplierID = @SupplierID AND (a.Status != 'Inactive' OR a.Status IS NULL)
                    ORDER BY a.AdvanceDate DESC";

                return conn.Query<Advance>(sql, new { SupplierID = supplierId }).ToList();
            }
        }
        public static decimal GetSupplierAdvanceTotalByMonth(int supplierId, int year, int month)
        {
            // Convert month name to month number
            ;

            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT COALESCE(SUM(Amount), 0) as TotalAdvance
            FROM Advances
            WHERE SupplierID = @SupplierID
            AND strftime('%Y', AdvanceDate) = @Year
            AND strftime('%m', AdvanceDate) = @Month
            AND (Status != 'Inactive' OR Status IS NULL)";

                return conn.QuerySingleOrDefault<decimal>(sql, new
                {
                    SupplierID = supplierId,
                    Year = year.ToString(),
                    Month = month.ToString("D2")
                });
            }
        }
        public static List<Advance> GetSupplierAdvanceByMonth(int supplierId, int year, int month)
        {
            // Convert month name to month number
            ;

            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT *
            FROM Advances
            WHERE SupplierID = @SupplierID
            AND strftime('%Y', AdvanceDate) = @Year
            AND strftime('%m', AdvanceDate) = @Month
            AND (Status != 'Inactive' OR Status IS NULL)";

                return conn.Query<Advance>(sql, new
                {
                    SupplierID = supplierId,
                    Year = year.ToString(),
                    Month = month.ToString("D2")
                }).ToList();
            }
        }

        public static int AddAdvance(Advance advance)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO Advances (SupplierID, AdvanceDate, Amount, Description, Status)
                    VALUES (@SupplierID, @AdvanceDate, @Amount, @Description, @Status);
                    SELECT last_insert_rowid();";

                int newId = conn.ExecuteScalar<int>(sql, advance);
                AuditManager.LogAction("Create", "Advances", newId.ToString(), $"Added Advance {advance.Amount} for Supplier {advance.SupplierID}");
                return newId;
            }
        }

        public static bool UpdateAdvance(Advance advance)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE Advances 
                    SET SupplierID = @SupplierID,
                        AdvanceDate = @AdvanceDate,
                        Amount = @Amount,
                        Description = @Description,
                        Status = @Status
                    WHERE AdvanceID = @AdvanceID";

                bool success = conn.Execute(sql, advance) > 0;
                if(success) AuditManager.LogAction("Update", "Advances", advance.AdvanceID.ToString(), $"Updated Advance ID {advance.AdvanceID}");
                return success;
            }
        }

        public static bool DeleteAdvance(int advanceId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                bool success = conn.Execute("Update Advances SET Status = 'Inactive' WHERE AdvanceID = @AdvanceID",
                    new { AdvanceID = advanceId }) > 0;
                if(success) AuditManager.LogAction("Delete", "Advances", advanceId.ToString(), "Soft Deleted Advance");
                return success;
            }
        }
        #endregion
        #region REPORTS & DASHBOARD
        // ========== REPORTS & DASHBOARD ==========
        public static decimal GetTodayCollectionTotal()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT COALESCE(SUM(Weight), 0) 
                    FROM DailyCollection 
                    WHERE date(CollectionDate) = date('now')";

                return conn.ExecuteScalar<decimal>(sql);
            }
        }

        public static decimal GetTodayCollectionAmount()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT COALESCE(SUM(TotalAmount), 0) 
                    FROM DailyCollection 
                    WHERE date(CollectionDate) = date('now')";

                return conn.ExecuteScalar<decimal>(sql);
            }
        }

        public static int GetActiveSuppliersCount()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM Suppliers WHERE Status = 'Active'");
            }
        }

        public static decimal GetMonthlyCollectionTotal()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT COALESCE(SUM(Weight), 0) 
                    FROM DailyCollection 
                    WHERE strftime('%Y-%m', CollectionDate) = strftime('%Y-%m', 'now')";

                return conn.ExecuteScalar<decimal>(sql);
            }
        }

        public static decimal GetMonthlyCollectionAmount()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT COALESCE(SUM(TotalAmount), 0) 
                    FROM DailyCollection 
                    WHERE strftime('%Y-%m', CollectionDate) = strftime('%Y-%m', 'now')";

                return conn.ExecuteScalar<decimal>(sql);
            }
        }

        public static decimal GetPendingPayments()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                // Calculate total advances not yet deducted
                string sql = @"
                    SELECT COALESCE(SUM(Amount), 0) 
                    FROM Advances 
                    WHERE Status = 'Active'";

                return conn.ExecuteScalar<decimal>(sql);
            }
        }

        public static List<DailyCollection> GetRecentActivities(int limit = 10)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT dc.*, s.SupplierName 
                    FROM DailyCollection dc
                    JOIN Suppliers s ON dc.SupplierID = s.SupplierID
                    ORDER BY dc.CreatedDate DESC
                    LIMIT @Limit";

                return conn.Query<DailyCollection>(sql, new { Limit = limit }).ToList();
            }
        }

        public static List<(string Month, decimal Amount)> GetMonthlyCollectionChart()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT 
                        strftime('%Y-%m', CollectionDate) as Month,
                        SUM(TotalAmount) as Amount
                    FROM DailyCollection
                    GROUP BY strftime('%Y-%m', CollectionDate)
                    ORDER BY Month DESC
                    LIMIT 6";

                return conn.Query<(string, decimal)>(sql).ToList();
            }
        }
        #endregion
        #region INVOICES
        // ========== INVOICES ==========
        public static List<MonthlyInvoice> GetAllInvoices()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT mi.*, s.SupplierName 
                    FROM MonthlyInvoices mi
                    JOIN Suppliers s ON mi.SupplierID = s.SupplierID
                    ORDER BY mi.InvoiceMonth DESC";

                return conn.Query<MonthlyInvoice>(sql).ToList();
            }
        }

        public static List<MonthlyInvoice> GetInvoicesByInvoiceNumber(string InvoiceID)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT 
                mi.*,
                s.SupplierName,
                s.SupplierNumber,
                s.DueAmount as DueAmount,
                l.LineName
            FROM MonthlyInvoices mi
            JOIN Suppliers s ON mi.SupplierID = s.SupplierID
            LEFT JOIN Lines l ON s.LineID = l.LineID
            WHERE mi.InvoiceId = @InvoiceID";

                return conn.Query<MonthlyInvoice>(sql, new
                {
                    InvoiceID,
                }).ToList();
            }
        }

        public static int GenerateInvoice(MonthlyInvoice invoice)
        {
            
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO MonthlyInvoices (InvoiceID,SupplierID, InvoiceMonth, TotalWeight, RatePerKg, TotalAmount, 
                                                TotalAdvances, PreviousBalance, NetAmount, Status,TransportFee,TransportAllowance, Year)
                    VALUES (@InvoiceID,@SupplierID, @InvoiceMonth, @TotalWeight, @RatePerKg, @TotalAmount, 
                            @TotalAdvances, @PreviousBalance, @NetAmount, @Status,@TransportFee,@TransportAllowance, @Year);
                    SELECT last_insert_rowid();";

                int newId = conn.ExecuteScalar<int>(sql, invoice);
                AuditManager.LogAction("Create", "MonthlyInvoices", newId.ToString(), $"Generated Invoice for Supplier {invoice.SupplierID}, Month {invoice.InvoiceMonth}");
                return newId;
            }
        }

        public static List<MonthlyInvoice> GetInvoicesBySup(string SupID)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT 
                mi.*,
                s.SupplierName,
                s.SupplierNumber,
                s.DueAmount  as DueAmount
            FROM MonthlyInvoices mi
            JOIN Suppliers s ON mi.SupplierID = s.SupplierID
            WHERE mi.SupplierID = @SupID";

                return conn.Query<MonthlyInvoice>(sql, new
                {
                    SupID,
                }).ToList();
            }
        }

        public static bool UpdateInvoice(MonthlyInvoice invoice)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
                    UPDATE MonthlyInvoices 
                    SET TotalWeight = @TotalWeight, 
                        RatePerKg = @RatePerKg, 
                        TotalAmount = @TotalAmount, 
                        TotalAdvances = @TotalAdvances, 
                        PreviousBalance = @PreviousBalance, 
                        NetAmount = @NetAmount, 
                        Status = @Status,
                        TransportFee = @TransportFee,
                        TransportAllowance = @TransportAllowance
                    WHERE InvoiceID = @InvoiceID";

                bool success = conn.Execute(sql, invoice) > 0;
                if (success) AuditManager.LogAction("Update", "MonthlyInvoices", invoice.InvoiceID, $"Updated Invoice {invoice.InvoiceID}");
                return success;
            }
        }

        #endregion
        #region UTILITY METHODS
        // ========== UTILITY METHODS ==========
        public static int GetNextSupplierNumber()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT MAX(CAST(SUBSTR(SupplierNumber, 3) AS INTEGER)) FROM Suppliers";
                var maxNum = conn.ExecuteScalar<int?>(sql);
                return (maxNum ?? 0) + 1;
            }
        }

        public static bool SupplierNumberExists(string supplierNumber)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Suppliers WHERE SupplierNumber = @SupplierNumber";
                return conn.ExecuteScalar<int>(sql, new { SupplierNumber = supplierNumber }) > 0;
            }
        }

        public static int GetCounts(string table)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                string sql = $"SELECT COUNT(LineID) AS count FROM {table};";
                var count = conn.ExecuteScalar<int>(sql);
                return count;
            }
        }


        public static void ExecuteTransaction(Action<SqliteConnection> action)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        action(conn);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static string GenerateInvoiceNumber(int year)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Get the last invoice number
                var sql = @"
        SELECT InvoiceID 
        FROM MonthlyInvoices 
        ORDER BY InvoiceID DESC 
        LIMIT 1";

                var lastNumber = conn.ExecuteScalar<string>(sql);

                if (string.IsNullOrEmpty(lastNumber))
                {
                    // First invoice: INV-2024-00001
                    return $"INV-{year}-00001";
                }

                // Extract number and increment
                if (lastNumber.Contains("-"))
                {
                    var parts = lastNumber.Split('-');

                    // Format: INV-2024-00001
                    // parts[0] = "INV"
                    // parts[1] = "2024" (year)
                    // parts[2] = "00001" (sequence number)

                    if (parts.Length >= 3 && int.TryParse(parts[2], out int sequenceNumber))
                    {
                        // Increment the sequence number
                        return $"{parts[0]}-{parts[1]}-{(sequenceNumber + 1):D5}";
                    }
                    else if (parts.Length == 2 && int.TryParse(parts[1], out int simpleNumber))
                    {
                        // Format: INV-00001 (no year)
                        return $"{parts[0]}-{(simpleNumber + 1):D5}";
                    }
                }

                // Default fallback - generate based on timestamp
                return $"INV-{DateTime.Now:yyyyMMddHHmmss}";
            }
        }
        #endregion
        #region Tea Packet
        // ===============Tea Packet==========================

        public static int AddTeaPacketSell(TeaPacket teaPacket)
        {
            using(var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    INSERT INTO TeaPacketSell (Supplier, Date, Price, Qty, Total)
                    VALUES (@SupllierID, @Date, @Price, @Qty, @Total);
                    ";
                return conn.ExecuteScalar<int>(sql, teaPacket);
            }
        }
        public static List<TeaPacket> GetTeaPackets()
        {
            using(var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                 SELECT ts.*,
                s.SupplierNumber,
                s.SupplierName
                 FROM TeaPacketSell ts
                 JOIN Suppliers s ON ts.Supplier = s.SupplierID
                ";
                return conn.Query<TeaPacket>(sql).ToList();
            }

        }

        public static List<TeaPacket> GetTeaPacketsBySup(string supId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                 SELECT ts.*,
                s.SupplierNumber,
                s.SupplierName
                 FROM TeaPacketSell ts
                 JOIN Suppliers s ON ts.Supplier = s.SupplierID
                WHERE ts.Supplier = @supID
                ";
                return conn.Query<TeaPacket>(sql, new { supID = supId }).ToList();
            }

        }

        #endregion
    }
}