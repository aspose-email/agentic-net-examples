using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using System;
using System.IO;

namespace AsposeEmailCsvReport
{
    class Program
    {
        static void Main()
        {
            try
            {
                string mboxPath = "storage.mbox";
                string csvPath = "report.csv";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"Input file not found: {mboxPath}");
                    return;
                }

                string csvDirectory = Path.GetDirectoryName(csvPath);
                if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
                {
                    Directory.CreateDirectory(csvDirectory);
                }

                using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    writer.WriteLine("EntryId,Subject,Date");

                    foreach (MboxMessageInfo messageInfo in mbox.EnumerateMessageInfo())
                    {
                        string entryId = messageInfo.EntryId ?? string.Empty;
                        string subject = messageInfo.Subject ?? string.Empty;
                        string dateStr = messageInfo.Date.ToString("o");

                        writer.WriteLine($"{EscapeCsv(entryId)},{EscapeCsv(subject)},{EscapeCsv(dateStr)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static string EscapeCsv(string field)
        {
            if (field == null)
                return string.Empty;

            bool mustQuote = field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r");
            if (mustQuote)
            {
                string escaped = field.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }
            return field;
        }
    }
}
