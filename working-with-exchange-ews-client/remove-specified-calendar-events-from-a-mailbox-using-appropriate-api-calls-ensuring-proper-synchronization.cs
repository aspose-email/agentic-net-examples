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
            // Initialize Gmail client with placeholder credentials
            IGmailClient client;
            try
            {
                client = GmailClient.GetInstance(
                    "clientId",          // Client ID
                    "clientSecret",      // Client Secret
                    "refreshToken",      // Refresh Token
                    "user@example.com"); // User Email
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly
            using (client)
            {
                // Identifier of the calendar to clean (e.g., "primary")
                string calendarId = "primary";

                // Retrieve all appointments from the specified calendar
                Appointment[] appointments;
                try
                {
                    appointments = client.ListAppointments(calendarId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list appointments: {ex.Message}");
                    return;
                }

                // Delete each appointment
                foreach (Appointment appt in appointments)
                {
                    try
                    {
                        // UniqueId holds the appointment identifier required for deletion
                        client.DeleteAppointment(calendarId, appt.UniqueId);
                        Console.WriteLine($"Deleted appointment with ID: {appt.UniqueId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete appointment {appt.UniqueId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
