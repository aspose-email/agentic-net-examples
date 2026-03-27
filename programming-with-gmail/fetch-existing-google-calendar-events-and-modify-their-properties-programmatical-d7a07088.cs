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
                // Initialize Gmail client with placeholder credentials
                IGmailClient gmailClient = GmailClient.GetInstance("clientId", "clientSecret", "refreshToken", "user@example.com");
                using (gmailClient)
                {
                    // Retrieve the list of calendars
                    Calendar[] calendars = gmailClient.ListCalendars();
                    if (calendars == null || calendars.Length == 0)
                    {
                        Console.WriteLine("No calendars found.");
                        return;
                    }

                    // Use the first calendar in the collection
                    string calendarId = calendars[0].Id;

                    // Retrieve appointments from the selected calendar
                    Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                    if (appointments == null || appointments.Length == 0)
                    {
                        Console.WriteLine("No appointments found in the selected calendar.");
                        return;
                    }

                    // Iterate through each appointment, modify its properties, and update it on Google Calendar
                    foreach (Appointment appointment in appointments)
                    {
                        // Example modifications
                        appointment.Summary = "Updated: " + appointment.Summary;
                        appointment.Description = (appointment.Description ?? string.Empty) + " (modified)";

                        // Push the changes back to Google Calendar
                        Appointment updated = gmailClient.UpdateAppointment(calendarId, appointment);
                        Console.WriteLine($"Updated appointment: {updated.Summary}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
