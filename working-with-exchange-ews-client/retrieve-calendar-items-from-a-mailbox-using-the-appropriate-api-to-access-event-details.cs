using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Retrieve the default calendar folder URI
                string calendarFolderUri = client.MailboxInfo.CalendarUri;

                // List appointments from the default calendar
                Appointment[] appointments = client.ListAppointments(calendarFolderUri);

                // Output basic details of each appointment
                foreach (Appointment appointment in appointments)
                {
                    Console.WriteLine("Subject: " + appointment.Summary);
                    Console.WriteLine("Start: " + appointment.StartDate);
                    Console.WriteLine("End: " + appointment.EndDate);
                    Console.WriteLine("Location: " + appointment.Location);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
