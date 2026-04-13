using System;
using System.Net;
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
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            ICredentials credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Get the default calendar folder URI
                string calendarFolderUri = client.MailboxInfo.CalendarUri;

                // Retrieve all appointments from the calendar
                Appointment[] appointments = client.ListAppointments(calendarFolderUri);

                // Filter cancelled appointments without attendees
                foreach (Appointment appointment in appointments)
                {
                    if (appointment.Status == AppointmentStatus.Cancelled &&
                        (appointment.Attendees == null || appointment.Attendees.Count == 0))
                    {
                        Console.WriteLine($"Cancelled appointment: {appointment.Summary}");
                        Console.WriteLine($"Start: {appointment.StartDate}, End: {appointment.EndDate}");
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
