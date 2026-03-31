using Aspose.Email.Calendar;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution in CI.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "user@example.com";

            // Guard against placeholder credentials to avoid external calls during CI.
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") || defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Initialize Gmail client.
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            using (gmailClient as IDisposable)
            {
                // Specify the calendar from which to delete events.
                string calendarId = "primary";

                // List of appointment IDs to be removed.
                List<string> appointmentIds = new List<string>
                {
                    "appointment-id-1",
                    "appointment-id-2"
                };

                // Delete each specified appointment.
                foreach (string appointmentId in appointmentIds)
                {
                    try
                    {
                        gmailClient.DeleteAppointment(calendarId, appointmentId);
                        Console.WriteLine($"Deleted appointment {appointmentId} from calendar {calendarId}.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete appointment {appointmentId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
