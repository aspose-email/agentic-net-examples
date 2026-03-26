using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Calendar;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client (replace with real token and email)
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            // Retrieve and display calendars
            Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
            Console.WriteLine("Calendars:");
            foreach (Aspose.Email.Clients.Google.Calendar calendar in calendars)
            {
                Console.WriteLine(" - Id: {0}, Summary: {1}", calendar.Id, calendar.Summary);

                // Retrieve and display appointments for each calendar
                Appointment[] appointments = gmailClient.ListAppointments(calendar.Id);
                foreach (Appointment appointment in appointments)
                {
                    Console.WriteLine("   * {0} ({1} - {2})", appointment.Summary, appointment.StartDate, appointment.EndDate);
                }
            }

            // TGZ file handling (replace with actual path)
            string tgzFilePath = "mailbox.tgz";

            if (!File.Exists(tgzFilePath))
            {
                Console.Error.WriteLine("TGZ file not found: " + tgzFilePath);
                return;
            }

            using (TgzReader tgzReader = new TgzReader(tgzFilePath))
            {
                Console.WriteLine("Messages in TGZ archive:");
                while (tgzReader.ReadNextMessage())
                {
                    MailMessage message = tgzReader.CurrentMessage;
                    if (message != null)
                    {
                        Console.WriteLine(" - Subject: {0}", message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}