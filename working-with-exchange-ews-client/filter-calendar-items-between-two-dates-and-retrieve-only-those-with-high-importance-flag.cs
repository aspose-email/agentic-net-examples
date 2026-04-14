using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client with safety handling
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Ensure client is disposed properly
            using (client)
            {
                // Define the date range for filtering
                DateTime filterStart = new DateTime(2023, 1, 1);
                DateTime filterEnd = new DateTime(2023, 12, 31);

                Appointment[] appointments = null;
                try
                {
                    // Retrieve all appointments from the default calendar folder
                    appointments = client.ListAppointments();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving appointments: {ex.Message}");
                    return;
                }

                // Filter appointments by date range and high importance
                foreach (Appointment appointment in appointments)
                {
                    if (appointment.StartDate >= filterStart &&
                        appointment.EndDate <= filterEnd &&
                        appointment.MicrosoftImportance == MSImportance.High)
                    {
                        Console.WriteLine($"Subject: {appointment.Summary}");
                        Console.WriteLine($"Start : {appointment.StartDate}");
                        Console.WriteLine($"End   : {appointment.EndDate}");
                        Console.WriteLine($"Importance: {appointment.MicrosoftImportance}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
