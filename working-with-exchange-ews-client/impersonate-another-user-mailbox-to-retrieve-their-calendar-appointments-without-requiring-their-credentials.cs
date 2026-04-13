using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string adminUsername = "admin@example.com";
            string adminPassword = "adminPassword";

            // User to impersonate
            string impersonatedUserEmail = "user@example.com";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, adminUsername, adminPassword))
            {
                try
                {
                    // Impersonate the target user
                    client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, impersonatedUserEmail);

                    // Get the calendar folder URI for the impersonated mailbox
                    string calendarFolderUri = client.MailboxInfo.CalendarUri;

                    // Retrieve appointments from the calendar
                    Appointment[] appointments = client.ListAppointments(calendarFolderUri);

                    Console.WriteLine($"Appointments for {impersonatedUserEmail}:");
                    foreach (Appointment appointment in appointments)
                    {
                        // Use Summary property (not Subject) to get the title
                        string title = appointment.Summary;
                        DateTime start = appointment.StartDate;
                        DateTime end = appointment.EndDate;
                        Console.WriteLine($"{start:G} - {end:G} : {title}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during impersonation or retrieval: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
