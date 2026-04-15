using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Source mailbox credentials
            string sourceMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential sourceCredentials = new NetworkCredential("sourceUser", "sourcePassword", "DOMAIN");

            // Target mailbox credentials
            string targetMailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential targetCredentials = new NetworkCredential("targetUser", "targetPassword", "DOMAIN");

            // Create source and target EWS clients
            using (IEWSClient sourceClient = EWSClient.GetEWSClient(sourceMailboxUri, sourceCredentials))
            using (IEWSClient targetClient = EWSClient.GetEWSClient(targetMailboxUri, targetCredentials))
            {
                // Ensure connections are established
                try
                {
                    // Access the default calendar folder URIs
                    string sourceCalendarUri = sourceClient.MailboxInfo.CalendarUri;
                    string targetCalendarUri = targetClient.MailboxInfo.CalendarUri;

                    // Retrieve all appointments from the source calendar
                    Appointment[] sourceAppointments = sourceClient.ListAppointments(sourceCalendarUri);

                    foreach (Appointment srcAppointment in sourceAppointments)
                    {
                        // Create the appointment in the target mailbox
                        // The CreateAppointment method returns the UID of the created item
                        string createdUid = targetClient.CreateAppointment(srcAppointment, targetCalendarUri);
                        Console.WriteLine($"Copied appointment '{srcAppointment.Summary}' with UID {createdUid}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
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
