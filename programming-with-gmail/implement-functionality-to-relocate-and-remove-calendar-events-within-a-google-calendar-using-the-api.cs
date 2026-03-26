using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the Gmail client (replace placeholders with real credentials)
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "YOUR_CLIENT_ID",
                "YOUR_CLIENT_SECRET",
                "YOUR_REFRESH_TOKEN",
                "user@example.com"))
            {
                // IDs of the source and destination calendars
                string sourceCalendarId = "source-calendar-id";
                string destinationCalendarId = "destination-calendar-id";

                // List appointments in the source calendar
                Appointment[] sourceAppointments = gmailClient.ListAppointments(sourceCalendarId);
                if (sourceAppointments == null || sourceAppointments.Length == 0)
                {
                    Console.WriteLine("No appointments found in the source calendar.");
                    return;
                }

                // Choose the first appointment to relocate
                Appointment appointmentToMove = sourceAppointments[0];
                string appointmentId = appointmentToMove.UniqueId;

                // Move the appointment to the destination calendar
                gmailClient.MoveAppointment(sourceCalendarId, appointmentId, destinationCalendarId);
                Console.WriteLine($"Appointment '{appointmentToMove.Summary}' moved to calendar '{destinationCalendarId}'.");

                // Optionally delete the moved appointment from the destination calendar
                gmailClient.DeleteAppointment(destinationCalendarId, appointmentId);
                Console.WriteLine($"Appointment '{appointmentToMove.Summary}' deleted from calendar '{destinationCalendarId}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}