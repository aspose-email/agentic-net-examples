using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Appointment identifiers
            string appointmentUniqueId = "AAMkAG..."; // replace with actual UID
            string calendarFolderUri = null; // null uses default calendar folder

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Cancel the appointment and send cancellation notifications to attendees
                    client.CancelAppointment(appointmentUniqueId, calendarFolderUri);
                    Console.WriteLine("Appointment cancelled and notifications sent.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error cancelling appointment: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
