using Aspose.Email.Storage.Pst;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("user@example.com", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Ensure the backup calendar folder exists; create if missing
                string backupFolderName = "BackupCalendar";

                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                ExchangeFolderInfo backupFolderInfo;
                bool folderExists = client.FolderExists(client.MailboxInfo.CalendarUri, backupFolderName, out backupFolderInfo);
                if (!folderExists)
                {
                    // Create a subfolder under the default Calendar folder
                    backupFolderInfo = client.CreateFolder(client.MailboxInfo.CalendarUri, backupFolderName);
                }

                // Retrieve appointments from the primary calendar
                Appointment[] appointments = client.ListAppointments(client.MailboxInfo.CalendarUri);

                // Forward each appointment to the backup calendar
                foreach (Appointment appointment in appointments)
                {
                    // Create a copy of the appointment in the backup folder
                    client.CreateAppointment(appointment, backupFolderInfo.Uri);
                }

                Console.WriteLine("Forwarded {0} appointments to backup calendar.", appointments.Length);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
