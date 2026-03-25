using System;
using System.IO;
using System.IO.Compression;
using System.Formats.Tar;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the TGZ file
            string tgzPath = "sample.tgz";

            // Verify TGZ file existence before processing
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"TGZ file not found: {tgzPath}");
                // Optionally, create a minimal placeholder TGZ file here if needed
            }
            else
            {
                // Process TGZ archive and list its entries
                using (FileStream tgzFileStream = File.OpenRead(tgzPath))
                using (GZipStream gzipStream = new GZipStream(tgzFileStream, CompressionMode.Decompress))
                using (TarReader tarReader = new TarReader(gzipStream))
                {
                    Console.WriteLine("Contents of TGZ archive:");
                    TarEntry entry;
                    while ((entry = tarReader.GetNextEntry()) != null)
                    {
                        Console.WriteLine($"- {entry.Name} ({entry.Length} bytes)");
                    }
                }
            }

            // Gmail client credentials (replace with actual values)
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Create Gmail client instance
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Fetch and display all contacts
                Contact[] contacts = gmailClient.GetAllContacts();
                Console.WriteLine($"Total contacts: {contacts.Length}");
                foreach (Contact contact in contacts)
                {
                    Console.WriteLine($"Contact: {contact.DisplayName}");
                }

                // Retrieve list of calendars
                Calendar[] calendars = gmailClient.ListCalendars();
                if (calendars.Length > 0)
                {
                    // Use the first calendar's identifier for demonstration
                    string calendarId = calendars[0].Id;

                    // Fetch and display appointments from the selected calendar
                    Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                    Console.WriteLine($"Appointments in calendar '{calendarId}': {appointments.Length}");
                    foreach (Appointment appointment in appointments)
                    {
                        Console.WriteLine($"- {appointment.Summary} at {appointment.StartDate}");
                    }
                }
                else
                {
                    Console.WriteLine("No calendars found for the account.");
                }
            }
        }
        catch (Exception ex)
        {
            // Global exception handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}