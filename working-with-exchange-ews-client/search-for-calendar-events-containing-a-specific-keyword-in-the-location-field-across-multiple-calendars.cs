using Aspose.Email.Calendar;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Keyword to search in the Location field
            string locationKeyword = "Conference";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // List of calendar folder URIs to search (default calendar + additional calendars)
            List<string> calendarFolderUris = new List<string>();

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Add the default calendar folder
                if (!string.IsNullOrEmpty(client.MailboxInfo.CalendarUri))
                {
                    calendarFolderUris.Add(client.MailboxInfo.CalendarUri);
                }

                // Example: add another calendar folder URI manually (replace with actual URI if needed)
                // calendarFolderUris.Add("https://exchange.example.com/EWS/Exchange.asmx/Calendars/TeamCalendar");

                foreach (string folderUri in calendarFolderUris)
                {
                    // Retrieve all appointments from the current calendar folder
                    Appointment[] appointments = client.ListAppointments(folderUri);

                    foreach (Appointment appt in appointments)
                    {
                        if (!string.IsNullOrEmpty(appt.Location) &&
                            appt.Location.IndexOf(locationKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.WriteLine("Found matching appointment:");
                            Console.WriteLine($"Subject : {appt.Summary}");
                            Console.WriteLine($"Location: {appt.Location}");
                            Console.WriteLine($"Start   : {appt.StartDate}");
                            Console.WriteLine($"End     : {appt.EndDate}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
