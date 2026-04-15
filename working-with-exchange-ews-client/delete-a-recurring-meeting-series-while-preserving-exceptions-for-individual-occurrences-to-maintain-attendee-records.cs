using Aspose.Email.Tools.Search;
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
            // Connection parameters – replace with actual values
            string hostUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (hostUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(hostUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Calendar folder URI
                string calendarUri = client.MailboxInfo.CalendarUri;

                // Retrieve all appointments in the calendar
                AppointmentCollection appointments;
                try
                {
                    appointments = client.ListAppointments(calendarUri, new MailQueryBuilder().GetQuery(), true);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
                    return;
                }

                // Identify the recurring meeting series to delete (example: by subject)
                foreach (Appointment appointment in appointments)
                {
                    // Check if this is a recurring series (Recurrence not null) and matches desired criteria
                    if (appointment.Recurrence != null && appointment.Summary == "Team Sync")
                    {
                        try
                        {
                            // Cancel the series; this keeps individual exceptions intact for attendees
                            client.CancelAppointment(appointment.UniqueId, calendarUri);
                            Console.WriteLine($"Cancelled recurring series: {appointment.Summary}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to cancel appointment '{appointment.Summary}': {ex.Message}");
                        }
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
