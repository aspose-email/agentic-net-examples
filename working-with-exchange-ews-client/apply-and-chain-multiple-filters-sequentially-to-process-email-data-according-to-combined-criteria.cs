using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using System;
using System.IO;

namespace EmailFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configuration for the Exchange EWS server
                string host = "exchange.example.com";
                string username = "user@example.com";
                string password = "password";
                string domain = "example";

                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") ||
                    username.Contains("example.com") ||
                    password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Output directory for filtered messages
                string outputDir = Path.Combine(Environment.CurrentDirectory, "FilteredEmails");

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Build the mail query with combined criteria
                // From contains 'test@test.com' OR Seen = True, AND SentDate >= 12-May-2010
                MailQuery mailQuery = new MailQuery("(('From' Contains 'test@test.com' | 'Seen' = 'True') & 'SentDate' >= '12-May-2010')");

                // Connect to the Exchange server and process messages
                try
                {
                    using (IEWSClient exchangeClient = EWSClient.GetEWSClient(host, username, password, domain))
                    {
                        // Retrieve messages that match the query from the Inbox folder
                        ExchangeMessageInfoCollection messages = exchangeClient.ListMessages("Inbox", mailQuery);

                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            try
                            {
                                // Fetch the full message
                                MailMessage message = exchangeClient.FetchMessage(messageInfo.UniqueUri);

                                // Save the message to the output directory
                                string filePath = Path.Combine(outputDir, $"{messageInfo.UniqueUri.GetHashCode()}.eml");
                                try
                                {
                                    message.Save(filePath);
                                    Console.WriteLine($"Saved filtered message to: {filePath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message {messageInfo.UniqueUri}: {saveEx.Message}");
                                }
                            }
                            catch (Exception fetchEx)
                            {
                                Console.Error.WriteLine($"Failed to fetch message {messageInfo.UniqueUri}: {fetchEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
