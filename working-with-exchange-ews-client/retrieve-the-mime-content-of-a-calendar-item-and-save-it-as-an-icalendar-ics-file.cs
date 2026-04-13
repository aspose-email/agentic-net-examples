using System;
using System.IO;
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
            // Exchange server connection details
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Folder and item identifiers for the calendar item
            // Adjust these values to point to an existing calendar item in your mailbox
            string calendarFolderUri;
            string calendarItemUri = "AAMkAD..."; // placeholder item URI

            // Output file path for the iCalendar (ICS) file
            string outputPath = Path.Combine(Environment.CurrentDirectory, "CalendarItem.ics");

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(exchangeUrl, new NetworkCredential(username, password)))
                {
                    // Get the calendar folder URI from the mailbox info
                    calendarFolderUri = client.MailboxInfo.CalendarUri;

                    // Fetch the appointment from the server
                    Appointment appointment = client.FetchAppointment(calendarFolderUri, calendarItemUri);

                    // Convert the appointment to a MIME message
                    MailMessage mimeMessage = appointment.ToMailMessage();

                    // Get the full MIME content as a string
                    string mimeContent = mimeMessage.ToString();

                    // Write the MIME content to the .ics file
                    try
                    {
                        File.WriteAllText(outputPath, mimeContent);
                        Console.WriteLine($"MIME content saved to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write MIME content to file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
