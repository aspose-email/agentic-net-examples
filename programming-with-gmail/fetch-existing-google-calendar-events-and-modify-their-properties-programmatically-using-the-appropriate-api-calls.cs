using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize Gmail client with dummy credentials
                IGmailClient gmailClient = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com");

                using (gmailClient)
                {
                    // Retrieve list of calendars
                    Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
                    if (calendars == null || calendars.Length == 0)
                    {
                        Console.WriteLine("No calendars found.");
                        return;
                    }

                    // Use the first calendar's identifier
                    string calendarId = calendars[0].Id;

                    // Retrieve appointments from the selected calendar
                    Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                    if (appointments == null || appointments.Length == 0)
                    {
                        Console.WriteLine("No appointments found in the calendar.");
                        return;
                    }

                    // Iterate through each appointment, modify a property, and update it on the server
                    foreach (Appointment appointment in appointments)
                    {
                        string originalSummary = appointment.Summary ?? string.Empty;
                        appointment.Summary = originalSummary + " - Updated";

                        // Update the appointment on Google Calendar
                        Appointment updatedAppointment = gmailClient.UpdateAppointment(calendarId, appointment);
                        Console.WriteLine($"Updated appointment: {updatedAppointment.Summary}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
