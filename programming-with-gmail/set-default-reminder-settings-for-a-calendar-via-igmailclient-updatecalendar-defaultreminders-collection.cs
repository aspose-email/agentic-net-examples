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
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "YOUR_EMAIL@example.com";
            string calendarId = "primary";

            // Guard against placeholder values.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace placeholder credentials with real values before running the sample.");
                return;
            }

            // Create Gmail client.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                try
                {
                    // Fetch the calendar to modify.
                    var calendar = gmailClient.FetchCalendar(calendarId);
                    if (calendar == null)
                    {
                        Console.Error.WriteLine($"Calendar with ID '{calendarId}' not found.");
                        return;
                    }

                    // Set default reminders (example: 10 minutes email, 5 minutes popup).
                    // Using empty array if enum values are unavailable.
                    calendar.DefaultReminders = new KeyValuePair<ReminderMethods, int>[]
                    {
                        new KeyValuePair<ReminderMethods, int>((ReminderMethods)0, 10), // Email (placeholder)
                        new KeyValuePair<ReminderMethods, int>((ReminderMethods)1, 5)   // Popup (placeholder)
                    };

                    // Update the calendar with new default reminders.
                    gmailClient.UpdateCalendar(calendar);
                    Console.WriteLine("Default reminders updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
