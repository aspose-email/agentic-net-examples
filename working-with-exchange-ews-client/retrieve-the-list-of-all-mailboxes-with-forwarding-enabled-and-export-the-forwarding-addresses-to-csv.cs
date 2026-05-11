using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsForwardingExport
{
    class Program
    {
        static void Main()
        {
            // ----- Configuration (replace with real values) -----
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard: skip real network calls when placeholders are detected.
            if (mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS connection.");
                Console.WriteLine("Export would be performed here if real credentials were provided.");
                return;
            }

            try
            {
                // Create and connect the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Retrieve all mailboxes from the Exchange server.
                    // The return type may vary between library versions (MailboxInfo[] or Contact[]),
                    // so we treat it as an object array and use reflection to access needed properties.
                    var mailboxes = client.GetMailboxes();

                    // Prepare CSV lines.
                    List<string> csvLines = new List<string>
                    {
                        "DisplayName,PrimarySmtpAddress,ForwardingAddress" // Header
                    };

                    foreach (var mb in mailboxes)
                    {
                        string displayName = GetStringProperty(mb, "DisplayName");
                        string primarySmtp = GetStringProperty(mb, "PrimarySmtpAddress");
                        string forwarding = GetStringProperty(mb, "ForwardingAddress");

                        if (string.IsNullOrEmpty(forwarding))
                            forwarding = GetStringProperty(mb, "ForwardingSmtpAddress");

                        if (!string.IsNullOrEmpty(forwarding))
                        {
                            string line = string.Format("{0},{1},{2}",
                                EscapeCsv(displayName),
                                EscapeCsv(primarySmtp),
                                EscapeCsv(forwarding));
                            csvLines.Add(line);
                        }
                    }

                    // Write CSV to file.
                    string outputPath = Path.Combine(Environment.CurrentDirectory, "forwarding_addresses.csv");
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    File.WriteAllLines(outputPath, csvLines);
                    Console.WriteLine("Export completed: " + outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }

        // Helper to safely retrieve string properties via reflection.
        private static string GetStringProperty(object obj, string propertyName)
        {
            if (obj == null) return string.Empty;
            PropertyInfo prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.PropertyType == typeof(string))
            {
                return prop.GetValue(obj) as string ?? string.Empty;
            }
            return string.Empty;
        }

        // Helper to escape commas and quotes for CSV fields.
        private static string EscapeCsv(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                string escaped = field.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }
            return field;
        }
    }
}
