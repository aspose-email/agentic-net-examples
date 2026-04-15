using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string outputIcsPath = "ExportedCalendar.ics";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            DateTime rangeStart = new DateTime(2023, 1, 1);
            DateTime rangeEnd = new DateTime(2023, 12, 31);

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputIcsPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create and connect EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (client)
            {
                // Retrieve appointments from the default calendar folder
                Appointment[] allAppointments;
                try
                {
                    allAppointments = client.ListAppointments(client.CurrentCalendarFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
                    return;
                }

                // Filter appointments by the specified date range
                List<Appointment> filtered = allAppointments
                    .Where(a => a.StartDate >= rangeStart && a.EndDate <= rangeEnd)
                    .ToList();

                // Export filtered appointments to an iCalendar file
                try
                {
                    using (var writer = new CalendarWriter(outputIcsPath))
                    {
                        foreach (var appt in filtered)
                        {
                            writer.Write(appt);
                        }
                    }
                    Console.WriteLine($"Exported {filtered.Count} appointments to '{outputIcsPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write iCalendar file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
