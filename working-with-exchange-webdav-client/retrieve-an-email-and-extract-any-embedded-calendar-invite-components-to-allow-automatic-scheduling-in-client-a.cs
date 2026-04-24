using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection details
                string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder credentials are used
                if (exchangeUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder Exchange credentials detected. Skipping network call.");
                    return;
                }

                // Create and connect the Exchange client safely
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    try
                    {
                        // List recent messages from the Inbox (limit to 10)
                        ExchangeMessageInfoCollection messageInfos = client.ListMessages(client.MailboxInfo.InboxUri, 10);

                        foreach (var info in messageInfos)
                        {
                            // Look for a message that likely contains a calendar invitation
                            if (info.MessageClass != null && info.MessageClass.Contains("IPM.Schedule.Meeting"))
                            {
                                // Retrieve the message URI as a string
                                string messageUri = info.UniqueUri;

                                // Fetch the full MAPI message using the URI
                                MapiMessage mapiMessage = client.FetchMapiMessage(messageUri);

                                // Verify that the message is a calendar item
                                if (mapiMessage.SupportedType == MapiItemType.Calendar)
                                {
                                    // Convert to MapiCalendar for easier access to calendar properties
                                    MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                                    // Display basic calendar information
                                    Console.WriteLine($"Subject: {calendar.Subject}");
                                    Console.WriteLine($"Start: {calendar.StartDate}");
                                    Console.WriteLine($"End: {calendar.EndDate}");
                                    Console.WriteLine($"Location: {calendar.Location}");

                                    // Optional: save the calendar as an iCalendar file
                                    string icsPath = "invite.ics";
                                    try
                                    {
                                        // Ensure the directory exists
                                        string dir = Path.GetDirectoryName(Path.GetFullPath(icsPath));
                                        if (!Directory.Exists(dir))
                                        {
                                            Directory.CreateDirectory(dir);
                                        }

                                        calendar.Save(icsPath);
                                        Console.WriteLine($"Calendar saved to {icsPath}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine($"Failed to save calendar file: {ex.Message}");
                                    }

                                    // Since we processed one invitation, exit the loop
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
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
}
