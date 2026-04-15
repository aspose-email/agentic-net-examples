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
            // EWS connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string projectCode = "PRJ123";

            // Folder and appointment identifiers
            string calendarFolderUri = "calendar";
            string appointmentUri = "appointment-unique-uri";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Fetch the existing appointment
                    Appointment appointment = client.FetchAppointment(appointmentUri, calendarFolderUri);

                    // Update the subject (summary) to include the project code
                    string originalSummary = appointment.Summary ?? string.Empty;
                    appointment.Summary = $"[{projectCode}] {originalSummary}";

                    // Send the updated appointment back to the server
                    client.UpdateAppointment(appointment, calendarFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
