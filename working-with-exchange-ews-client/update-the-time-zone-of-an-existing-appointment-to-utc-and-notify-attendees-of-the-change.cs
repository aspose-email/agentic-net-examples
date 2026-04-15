using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with actual credentials and service URL)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);

            // Folder that contains the appointment (calendar folder)
            string calendarFolderUri = client.MailboxInfo.CalendarUri;

            // Unique identifier of the appointment to be updated
            string appointmentId = "AAMkAD..."; // replace with actual appointment ID

            // Fetch the existing appointment
            Appointment appointment = client.FetchAppointment(calendarFolderUri, appointmentId);
            if (appointment == null)
            {
                Console.Error.WriteLine("Appointment not found.");
                return;
            }

            // Update the appointment's time zone to UTC
            appointment.SetTimeZone("UTC");

            // Save the changes back to the server; this will notify attendees of the change
            client.UpdateAppointment(appointment, calendarFolderUri);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
