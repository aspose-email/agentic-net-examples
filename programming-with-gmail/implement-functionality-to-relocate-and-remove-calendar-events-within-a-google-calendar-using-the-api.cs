using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId", "clientSecret", "refreshToken", "user@example.com"))
            {
                string sourceCalendarId = "sourceCalendarId";
                string targetCalendarId = "targetCalendarId";

                // Retrieve appointments from the source calendar
                Appointment[] appointments = gmailClient.ListAppointments(sourceCalendarId);
                if (appointments == null || appointments.Length == 0)
                {
                    Console.WriteLine("No appointments found in the source calendar.");
                    return;
                }

                // Select the first appointment to relocate
                Appointment appointmentToMove = appointments[0];
                string appointmentId = appointmentToMove.UniqueId;

                // Relocate the appointment to the target calendar
                gmailClient.MoveAppointment(sourceCalendarId, appointmentId, targetCalendarId);
                Console.WriteLine($"Moved appointment {appointmentId} to calendar {targetCalendarId}.");

                // Optionally delete the moved appointment from the target calendar
                Appointment movedAppointment = gmailClient.FetchAppointment(targetCalendarId, appointmentId);
                if (movedAppointment != null)
                {
                    gmailClient.DeleteAppointment(targetCalendarId, appointmentId);
                    Console.WriteLine($"Deleted appointment {appointmentId} from calendar {targetCalendarId}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
