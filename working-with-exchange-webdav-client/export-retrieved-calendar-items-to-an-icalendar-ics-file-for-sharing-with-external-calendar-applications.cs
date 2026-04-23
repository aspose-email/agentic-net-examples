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
            // Placeholder credentials and server URI
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Output directory for .ics files
            string outputDir = Path.Combine(Environment.CurrentDirectory, "ExportedCalendars");
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Connect to Exchange using WebDAV client
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        try
                        {
                            // Fetch the MAPI message
                            MapiMessage mapiMessage = client.FetchMapiMessage(messageInfo.UniqueUri);
                            if (mapiMessage == null)
                                continue;

                            // Process only calendar items
                            if (mapiMessage.SupportedType == MapiItemType.Calendar)
                            {
                                // Convert to MapiCalendar
                                MapiCalendar calendar = mapiMessage.ToMapiMessageItem() as MapiCalendar;
                                if (calendar == null)
                                    continue;

                                // Build a safe file name
                                string safeSubject = string.IsNullOrWhiteSpace(calendar.Subject) ? "Untitled" : calendar.Subject;
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(c, '_');
                                }
                                string icsPath = Path.Combine(outputDir, $"{safeSubject}_{Guid.NewGuid()}.ics");

                                // Save as iCalendar (.ics)
                                try
                                {
                                    calendar.Save(icsPath);
                                    Console.WriteLine($"Exported calendar to: {icsPath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save calendar '{calendar.Subject}': {saveEx.Message}");
                                }
                            }
                        }
                        catch (Exception msgEx)
                        {
                            Console.Error.WriteLine($"Error processing message '{messageInfo.UniqueUri}': {msgEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
