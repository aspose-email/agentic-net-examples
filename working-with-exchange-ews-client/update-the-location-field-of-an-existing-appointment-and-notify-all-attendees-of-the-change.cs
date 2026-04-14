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
            // Initialize EWS client
            IEWSClient client;
            try
            {
                // Replace with actual service URL, username and password
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the calendar folder URI from the mailbox info
            string calendarFolderUri;
            try
            {
                calendarFolderUri = client.MailboxInfo.CalendarUri;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to obtain calendar folder URI: {ex.Message}");
                return;
            }

            // UID of the appointment to be updated (replace with actual value)
            string appointmentUri = "AAMkAD..."; // placeholder

            // Fetch the existing appointment
            Appointment appointment;
            try
            {
                appointment = client.FetchAppointment(appointmentUri, calendarFolderUri);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to fetch appointment: {ex.Message}");
                return;
            }

            // Update the location
            appointment.Location = "Conference Room 2";

            // Save changes and notify attendees
            try
            {
                client.UpdateAppointment(appointment, calendarFolderUri);
                Console.WriteLine("Appointment location updated and attendees notified.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to update appointment: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
