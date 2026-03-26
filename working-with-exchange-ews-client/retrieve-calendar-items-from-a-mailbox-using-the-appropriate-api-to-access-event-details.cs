using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create credentials object
                NetworkCredential credentials = new NetworkCredential(username, password);

                // Initialize EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Retrieve the calendar folder URI from mailbox info
                        string calendarFolderUri = client.MailboxInfo.CalendarUri;

                        // List appointments from the calendar folder
                        Appointment[] appointments = client.ListAppointments(calendarFolderUri);

                        // Output appointment details
                        Console.WriteLine($"Found {appointments.Length} appointment(s):");
                        foreach (Appointment appt in appointments)
                        {
                            Console.WriteLine("--------------------------------------------------");
                            Console.WriteLine($"Subject   : {appt.Summary}");
                            Console.WriteLine($"Location  : {appt.Location}");
                            Console.WriteLine($"Start     : {appt.StartDate}");
                            Console.WriteLine($"End       : {appt.EndDate}");
                            Console.WriteLine($"Organizer : {appt.Organizer?.Address}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while retrieving appointments: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
                return;
            }
        }
    }
}