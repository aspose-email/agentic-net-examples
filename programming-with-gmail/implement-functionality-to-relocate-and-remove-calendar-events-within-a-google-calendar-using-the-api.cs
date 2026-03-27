using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

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

            // List all calendars
            Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();

            if (calendars == null || calendars.Length < 2)
            {
                Console.Error.WriteLine("At least two calendars are required to relocate events.");
                return;
            }

            // Choose source and destination calendars (first two in the list)
            string sourceCalendarId = calendars[0].Id;
            string destinationCalendarId = calendars[1].Id;

            // Retrieve appointments from the source calendar
            Appointment[] sourceAppointments = gmailClient.ListAppointments(sourceCalendarId);

            if (sourceAppointments == null || sourceAppointments.Length == 0)
            {
                Console.WriteLine("No appointments found in the source calendar.");
                return;
            }

            // Relocate each appointment to the destination calendar
            foreach (Appointment appointment in sourceAppointments)
            {
                // Unique identifier of the appointment
                string appointmentId = appointment.UniqueId;

                // Move the appointment
                Appointment movedAppointment = gmailClient.MoveAppointment(
                    sourceCalendarId,
                    destinationCalendarId,
                    appointmentId);

                Console.WriteLine($"Moved appointment '{movedAppointment.Summary}' to calendar '{calendars[1].Summary}'.");
            }

            // Example: delete a specific appointment from the destination calendar
            // (deleting the first moved appointment as a demonstration)
            string deleteAppointmentId = sourceAppointments[0].UniqueId;
            gmailClient.DeleteAppointment(destinationCalendarId, deleteAppointmentId);
            Console.WriteLine($"Deleted appointment with ID '{deleteAppointmentId}' from the destination calendar.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
