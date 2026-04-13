using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace ForwardMeetingInvitations
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "your_username";
                string password = "your_password";

                // URI of the secondary calendar folder where invitations will be backed up.
                // This should be the folder URI obtained from the server (e.g., via GetFolderInfo).
                string backupCalendarFolderUri = "backup-calendar-folder-uri";

                // Simple guard to avoid external calls when placeholders are used.
                if (string.IsNullOrWhiteSpace(mailboxUri) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    mailboxUri.Contains("your", StringComparison.OrdinalIgnoreCase) ||
                    username.Contains("your", StringComparison.OrdinalIgnoreCase) ||
                    password.Contains("your", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                    return;
                }

                // Create the EWS client.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Get the default calendar folder URI.
                        string defaultCalendarUri = client.MailboxInfo.CalendarUri;

                        // Retrieve all appointments from the default calendar.
                        Appointment[] appointments = client.ListAppointments(defaultCalendarUri);

                        foreach (Appointment appointment in appointments)
                        {
                            try
                            {
                                // Create a copy of the appointment in the backup calendar.
                                // The CreateAppointment method returns the UID of the created item.
                                client.CreateAppointment(appointment, backupCalendarFolderUri);
                                Console.WriteLine($"Backed up appointment: {appointment.Summary}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to back up appointment '{appointment.Summary}': {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
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
