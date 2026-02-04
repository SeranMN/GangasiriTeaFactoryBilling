using GangasiriTeaFactoryBilling.db;
using GangasiriTeaFactoryBilling.Invoice;
using GangasiriTeaFactoryBilling.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.PdfHelper
{
    public class CreateInvoicePDF
    {
        private readonly MonthlyInvoice _invoice;

        public CreateInvoicePDF(MonthlyInvoice invoice)
        {
            _invoice = invoice;
        }


        public string GenerateInvoicePDF(string receiptNo)
        {
            // Create folder for invoices if it doesn't exist
            string invoicesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Supplier Invoice");
            if (!Directory.Exists(invoicesFolder))
            {
                Directory.CreateDirectory(invoicesFolder);
            }

            // Generate unique filename
            string fileName = $"Invoice_{receiptNo}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(invoicesFolder, fileName);
            var document = new InvoiceDocument(_invoice);
            document.GeneratePdf(filePath);

            return filePath;
        }
    }

    public class InvoiceDocument : IDocument
    {
        private readonly MonthlyInvoice _invoice;

        public InvoiceDocument(MonthlyInvoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.MarginTop(20);
                    page.MarginBottom(20);
                    page.MarginHorizontal(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Noto Sans Sinhala"));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().AlignCenter().Text("ගඟසිරි තේ කර්මාන්තශාලාව").Bold().FontSize(14).FontColor("#006400");
                column.Item().AlignCenter().Text("වීරපාන, ඕපාත");
                column.Item().AlignCenter().Text("O : 0763169992 F : 0763169993 M : 076319994");

                column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                column.Item().AlignCenter().Text("මාසික ඉන්වොයිසිය").Bold().FontSize(14).FontColor("#006400");

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell().Text($"අංකය: {_invoice.InvoiceID}");
                    table.Cell().AlignRight().Text($"දිනය: {DateTime.Now:dd/MM/yyyy}");
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    ComposeSupplierDetails(column);
                    ComposeTeaLeavesDetails(column);
                    ComposeTransportDetails(column);
                    ComposeTotalAmount(column);
                    ComposeDeductions(column);
                    ComposeNetAmount(column);
                    ComposePaymentMode(column);
                });

                row.ConstantItem(10);

                row.RelativeItem().Column(column =>
                {
                    ComposeDateWiseDetails(column);
                });
            });
        }

        private void ComposeSupplierDetails(ColumnDescriptor column)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(100);
                    columns.RelativeColumn();
                });

                table.Cell().Text("මාසය:").Bold();
                table.Cell().Text(_invoice.InvoiceMonth);

                table.Cell().Text("සැපයුම්කරුගේ අංකය").Bold();
                table.Cell().Text(_invoice.SupplierID.ToString());

                table.Cell().Text("සැපයුම්කරුගේ නම").Bold();
                table.Cell().Text(_invoice.SupplierName);

                table.Cell().Text("පේළිය").Bold();
                table.Cell().Text(_invoice.LineName ?? "N/A");
            });

            column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Medium);
        }

        private void ComposeTeaLeavesDetails(ColumnDescriptor column)
        {
            decimal teaAmount = _invoice.RatePerKg * _invoice.TotalWeight;

            column.Item().Text("තේ දළු විස්තර").Bold();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Text("බර:");
                table.Cell().AlignRight().Text($"{_invoice.TotalWeight} kg");

                table.Cell().Text("වටිනාකම:");
                table.Cell().AlignRight().Text($"{_invoice.RatePerKg:N2}");

                table.Cell().Text("ශුද්ධ වටිනාකම:").Bold();
                table.Cell().AlignRight().Text($"Rs. {teaAmount:N2}").FontColor(Colors.Blue.Medium).Bold();
            });
        }

        private void ComposeTransportDetails(ColumnDescriptor column)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Text("ප්‍රවාහන දීමනා:");
                table.Cell().AlignRight().Text($"Rs. {_invoice.TransportAllowance:N2}");
            });
        }

        private void ComposeTotalAmount(ColumnDescriptor column)
        {
            decimal teaAmount = _invoice.RatePerKg * _invoice.TotalWeight;
            decimal totalAmount = _invoice.TransportAllowance + teaAmount;

            column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Medium);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Text("මෙම මාසය සඳහා ගෙවීම්:").Bold().FontSize(10);
                table.Cell().AlignRight().Text($"Rs. {totalAmount:N2}").FontColor(Colors.Blue.Medium).Bold().FontSize(10);
            });
            column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Medium);
        }

        private void ComposeDeductions(ColumnDescriptor column)
        {
            column.Item().Text("ගාස්තු අඩු කිරීම්").Bold();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Text("ප්‍රවාහන ගාස්තු :");
                table.Cell().AlignRight().Text($"Rs. {_invoice.TransportFee:N2}");

                table.Cell().Text("කලින් ගෙනා ශේෂය:");
                table.Cell().AlignRight().Text($"Rs. {_invoice.PreviousBalance:N2}");

                table.Cell().Text("ලබාගත් අත්තිකාරම්:");
                table.Cell().AlignRight().Text($"Rs. {_invoice.TotalAdvances:N2}");
            });
        }

        private void ComposeNetAmount(ColumnDescriptor column)
        {
            decimal teaAmount = _invoice.RatePerKg * _invoice.TotalWeight;
            decimal totalAmount = _invoice.TransportAllowance + teaAmount;
            decimal totalDeductions = _invoice.TotalAdvances + _invoice.PreviousBalance + _invoice.TransportFee;
            decimal netAmount = totalAmount - totalDeductions;

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().Text("එකතුව:");
                table.Cell().AlignRight().Text($"Rs. {totalDeductions:N2}").FontColor(Colors.Red.Medium).Bold();

                table.Cell().Text("ශුද්ධ ගෙවීම :").Bold().FontSize(10);
                table.Cell().AlignRight().Text($"Rs. {netAmount:N2}").FontColor(Colors.Green.Medium).Bold().FontSize(10);
            });
            column.Item().PaddingVertical(2).LineHorizontal(1).LineColor(Colors.Grey.Medium);
        }

        private void ComposePaymentMode(ColumnDescriptor column)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(100);
                    columns.RelativeColumn();
                });

                table.Cell().Text("ගෙවීමේ ක්‍රමය:").Bold();
                table.Cell().Text("මුදලින්");
            });
        }

        private void ComposeDateWiseDetails(ColumnDescriptor column)
        {
            column.Item().AlignCenter().Text("මාසය තුළ ගෙවීම් විස්තර").Bold().FontSize(10);

            var collectionEntries = DataAccess.GetSupplierWeightByMonth(_invoice.SupplierID, _invoice.Year, GetMonthNumber(_invoice.InvoiceMonth));
            var advanceEntries = DataAccess.GetSupplierAdvanceByMonth(_invoice.SupplierID, _invoice.Year, GetMonthNumber(_invoice.InvoiceMonth));
            var dateWiseData = CombineDateWiseData(collectionEntries, advanceEntries);

            var month = GetMonthNumber(_invoice.InvoiceMonth);
            var year = _invoice.Year;
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var allDaysData = new List<DateWiseEntry>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                var entry = dateWiseData.FirstOrDefault(d => d.Date.Date == date.Date);
                if (entry != null)
                {
                    allDaysData.Add(entry);
                }
                else
                {
                    allDaysData.Add(new DateWiseEntry { Date = date, TeaLeavesWeight = 0, AdvanceAmount = 0 });
                }
            }


            if (!allDaysData.Any())
            {
                column.Item().AlignCenter().Text("No date-wise data available");
                return;
            }

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("දිනය");
                    header.Cell().Element(CellStyle).Text("තේ දළු බර");
                    header.Cell().Element(CellStyle).Text("අත්තිකාරම්");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.Bold()).PaddingVertical(2).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                decimal totalWeight = 0;
                decimal totalAdvances = 0;

                foreach (var entry in allDaysData.OrderBy(d => d.Date))
                {
                    table.Cell().Element(CellStyle).AlignCenter().Text(entry.Date.ToString("dd/MM/yyyy"));
                    table.Cell().Element(CellStyle).AlignCenter().Text(entry.TeaLeavesWeight > 0 ? $"{entry.TeaLeavesWeight} kg" : "-");
                    table.Cell().Element(CellStyle).AlignCenter().Text(entry.AdvanceAmount > 0 ? $"Rs. {entry.AdvanceAmount:N2}" : "-");
                    
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(1);
                    }

                    totalWeight += entry.TeaLeavesWeight;
                    totalAdvances += entry.AdvanceAmount;
                }

                table.Cell().Element(FooterStyle).AlignRight().Text("එකතුව");
                table.Cell().Element(FooterStyle).AlignCenter().Text($"{totalWeight} kg");
                table.Cell().Element(FooterStyle).AlignCenter().Text($"Rs. {totalAdvances:N2}");

                static IContainer FooterStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.Bold()).BorderTop(1).BorderColor(Colors.Black).PaddingVertical(1);
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignCenter().Text(".................................................\nසැපයුම්කරුගේ අත්සන");
                    row.RelativeItem().AlignCenter().Text(".................................................\nකළමනාකරුගේ අත්සන");
                });

                column.Item().PaddingTop(5).AlignCenter().Text("මෙය සවිඥානයෙන් යුතුව බැලූ බැල්මට නිවැරදි බව සනාථ කරමි.");
                column.Item().AlignCenter().Text($"උත්පාදනය කල දිනය: {DateTime.Now:dd/MM/yyyy HH:mm}");
            });
        }
        private int GetMonthNumber(string monthName)
        {
            // Same method as before
            switch (monthName.ToLower())
            {
                case "january": case "ජනවාරි": return 1;
                case "february": case "පෙබරවාරි": return 2;
                case "march": case "මාර්තු": return 3;
                case "april": case "අප්‍රේල්": return 4;
                case "may": case "මැයි": return 5;
                case "june": case "ජුනි": return 6;
                case "july": case "ජූලි": return 7;
                case "august": case "අගෝස්තු": return 8;
                case "september": case "සැප්තැම්බර්": return 9;
                case "october": case "ඔක්තෝබර්": return 10;
                case "november": case "නොවැම්බර්": return 11;
                case "december": case "දෙසැම්බර්": return 12;
                default: return DateTime.Now.Month;
            }
        }
        private List<DateWiseEntry> CombineDateWiseData(List<DailyCollection> collectionEntries, List<Advance> advanceEntries)
        {
            // Same method as before
            var dateWiseData = new List<DateWiseEntry>();
            var allDates = new HashSet<DateTime>();

            if (collectionEntries != null)
            {
                foreach (var collection in collectionEntries)
                {
                    allDates.Add(collection.CollectionDate.Date);
                }
            }

            if (advanceEntries != null)
            {
                foreach (var advance in advanceEntries)
                {
                    allDates.Add(advance.AdvanceDate.Date);
                }
            }

            foreach (var date in allDates.OrderBy(d => d))
            {
                var entry = new DateWiseEntry
                {
                    Date = date,
                    TeaLeavesWeight = 0,
                    AdvanceAmount = 0
                };

                if (collectionEntries != null)
                {
                    var collection = collectionEntries.FirstOrDefault(c => c.CollectionDate.Date == date.Date);
                    if (collection != null)
                    {
                        entry.TeaLeavesWeight = collection.Weight;
                    }
                }

                if (advanceEntries != null)
                {
                    var advance = advanceEntries.FirstOrDefault(a => a.AdvanceDate.Date == date.Date);
                    if (advance != null)
                    {
                        entry.AdvanceAmount = advance.Amount;
                    }
                }

                dateWiseData.Add(entry);
            }

            return dateWiseData;
        }

    }
}