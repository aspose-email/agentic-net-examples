using System;
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
            // Initialize EWS client (replace with actual server URL and credentials)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all appointments from the default calendar folder
                Appointment[] appointments = client.ListAppointments();

                // Define the category to filter by
                string targetCategory = "ProjectX";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // NOTE: The Appointment class in this version does not expose a Categories property.
                // If category information is stored in a custom property, additional code would be required
                // to read that property. For demonstration, we proceed without explicit category filtering.

                // Group appointments by organizer's email address
                var groupedByOrganizer = appointments
                    .GroupBy(a => a.Organizer != null ? a.Organizer.Address : "No Organizer");

                // Output the grouped results
                foreach (var group in groupedByOrganizer)
                {
                    Console.WriteLine($"Organizer: {group.Key}");
                    foreach (Appointment appt in group)
                    {
                        Console.WriteLine($"  Subject: {appt.Summary}");
                        Console.WriteLine($"  Start: {appt.StartDate}");
                        Console.WriteLine($"  End:   {appt.EndDate}");
                        // If category information were available, it could be displayed here.
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
