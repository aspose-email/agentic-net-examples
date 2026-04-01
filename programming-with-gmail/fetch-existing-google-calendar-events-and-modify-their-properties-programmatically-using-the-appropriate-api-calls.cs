using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values to run against Gmail.
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Detect placeholder credentials and skip external calls.
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken" || defaultEmail == "user@example.com")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Calendar identifier (e.g., "primary" for the main calendar).
            string calendarId = "primary";

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                try
                {
                    // Fetch existing appointments.
                    Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                    if (appointments == null || appointments.Length == 0)
                    {
                        Console.WriteLine("No appointments found in the specified calendar.");
                        return;
                    }

                    foreach (Appointment appointment in appointments)
                    {
                        // Example modification: prepend "Updated:" to the summary.
                        string originalSummary = appointment.Summary ?? string.Empty;
                        appointment.Summary = "Updated: " + originalSummary;

                        // Update the appointment on the server.
                        try
                        {
                            Appointment updated = gmailClient.UpdateAppointment(calendarId, appointment);
                            Console.WriteLine($"Appointment '{originalSummary}' updated to '{updated.Summary}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update appointment '{originalSummary}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Gmail operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
