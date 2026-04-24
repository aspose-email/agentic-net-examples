using Aspose.Email.Calendar;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and identifiers.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string sourceCalendarId = "SOURCE_CALENDAR_ID";
            string destinationCalendarId = "DESTINATION_CALENDAR_ID";
            string appointmentId = "APPOINTMENT_ID";

            // Skip execution when placeholders are not replaced.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_") ||
                string.IsNullOrWhiteSpace(sourceCalendarId) || sourceCalendarId.StartsWith("SOURCE_") ||
                string.IsNullOrWhiteSpace(destinationCalendarId) || destinationCalendarId.StartsWith("DESTINATION_") ||
                string.IsNullOrWhiteSpace(appointmentId) || appointmentId.StartsWith("APPOINTMENT_"))
            {
                Console.Error.WriteLine("Placeholder values detected. Replace them with real credentials and IDs before running.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Move the appointment from source calendar to destination calendar.
                    Appointment movedAppointment = gmailClient.MoveAppointment(sourceCalendarId, destinationCalendarId, appointmentId);
                    Console.WriteLine("Appointment moved successfully. New summary: " + movedAppointment.Summary);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error moving appointment: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
