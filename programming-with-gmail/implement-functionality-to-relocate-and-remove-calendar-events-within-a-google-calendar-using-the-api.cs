using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace RelocateAndRemoveGoogleCalendarEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values when running against a live account.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string userEmail = "user@example.com";

                // Guard against executing live calls with placeholder credentials.
                if (clientId == "clientId" || clientSecret == "clientSecret" ||
                    refreshToken == "refreshToken" || userEmail == "user@example.com")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                    return;
                }

                // Create the Gmail client.
                IGmailClient gmailClient;
                try
                {
                    gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, userEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                // Ensure the client is disposed properly.
                using (gmailClient as IDisposable)
                {
                    // Identifiers for source and destination calendars and appointments.
                    string sourceCalendarId = "source-calendar-id";
                    string destinationCalendarId = "destination-calendar-id";
                    string appointmentIdToMove = "appointment-id-to-move";
                    string appointmentIdToDelete = "appointment-id-to-delete";

                    // Relocate (move) an appointment from the source calendar to the destination calendar.
                    try
                    {
                        gmailClient.MoveAppointment(sourceCalendarId, appointmentIdToMove, destinationCalendarId);
                        Console.WriteLine($"Appointment '{appointmentIdToMove}' moved from calendar '{sourceCalendarId}' to '{destinationCalendarId}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move appointment: {ex.Message}");
                    }

                    // Remove (delete) an appointment from the source calendar.
                    try
                    {
                        gmailClient.DeleteAppointment(sourceCalendarId, appointmentIdToDelete);
                        Console.WriteLine($"Appointment '{appointmentIdToDelete}' deleted from calendar '{sourceCalendarId}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete appointment: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
