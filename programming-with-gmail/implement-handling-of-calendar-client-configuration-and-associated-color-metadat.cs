using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;

namespace AsposeEmailGmailSample
{
    class Program
    {
        static void Main()
        {
            // Top-level exception guard
            try
            {
                // Initialize Gmail client with dummy OAuth token and default email
                using (IGmailClient gmailClient = GmailClient.GetInstance(
                    accessToken: "dummy_access_token",
                    defaultEmail: "user@example.com"))
                {
                    // Retrieve and display color information
                    try
                    {
                        ColorsInfo colorsInfo = gmailClient.GetColors();
                        // ColorsInfo may contain a collection of colors; display its string representation
                        Console.WriteLine("Colors Info: " + (colorsInfo?.ToString() ?? "No colors returned"));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error retrieving colors: " + ex.Message);
                    }

                    // List messages in the mailbox and display their subjects
                    try
                    {
                        List<GmailMessageInfo> messages = gmailClient.ListMessages();
                        Console.WriteLine($"Total messages: {messages?.Count ?? 0}");
                        foreach (GmailMessageInfo msgInfo in messages)
                        {
                            // Fetch the full message to access the subject
                            MailMessage fullMessage = gmailClient.FetchMessage(msgInfo.Id);
                            Console.WriteLine("Subject: " + (fullMessage?.Subject ?? "(no subject)"));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error listing messages: " + ex.Message);
                    }

                    // List appointments for a specific calendar (using "primary" as a placeholder)
                    try
                    {
                        string calendarId = "primary";
                        Appointment[] appointments = gmailClient.ListAppointments(calendarId);
                        Console.WriteLine($"Total appointments in calendar '{calendarId}': {appointments?.Length ?? 0}");
                        foreach (Appointment appt in appointments)
                        {
                            Console.WriteLine("Appointment Summary: " + (appt?.Summary ?? "(no summary)"));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error listing appointments: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Global exception handling
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
