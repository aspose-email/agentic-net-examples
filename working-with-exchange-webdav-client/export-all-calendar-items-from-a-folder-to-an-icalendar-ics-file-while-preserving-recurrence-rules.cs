using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Output folder for the exported .ics files.
            string outputFolder = "ExportedCalendars";

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to Exchange using WebDAV client.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Attempt to list messages in the Calendar folder.
                    // The CalendarUri property provides the default calendar folder URI.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.CalendarUri);

                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        try
                        {
                            // Fetch the full MAPI message.
                            MapiMessage mapiMessage = client.FetchMapiMessage(messageInfo.UniqueUri);

                            // Process only calendar items.
                            if (mapiMessage.SupportedType == MapiItemType.Calendar)
                            {
                                // Convert to a MapiCalendar object.
                                MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                                // Determine a file name – use the UID if available.
                                string uid = calendar.Uid;
                                if (string.IsNullOrEmpty(uid))
                                {
                                    uid = Guid.NewGuid().ToString();
                                }

                                string icsPath = Path.Combine(outputFolder, $"{uid}.ics");

                                // Save the calendar item as iCalendar, preserving recurrence rules.
                                calendar.Save(icsPath, new MapiCalendarIcsSaveOptions());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to process message '{messageInfo.UniqueUri}': {ex.Message}");
                            // Continue with next message.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
