using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                // Replace with actual mailbox URI and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                ICredentials credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure proper disposal
            using (client as IDisposable)
            {
                // Get the Calendar folder URI
                string calendarFolderUri = client.MailboxInfo.CalendarUri;

                // Retrieve appointments from the Calendar folder
                Appointment[] appointments = null;
                try
                {
                    appointments = client.ListAppointments(calendarFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
                    return;
                }

                // Display basic information about each appointment
                foreach (Appointment appt in appointments)
                {
                    // Appointment class uses Summary for the subject line
                    Console.WriteLine($"Subject: {appt.Summary}");
                    Console.WriteLine($"Start: {appt.StartDate}");
                    Console.WriteLine($"End:   {appt.EndDate}");
                    Console.WriteLine($"Location: {appt.Location}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
