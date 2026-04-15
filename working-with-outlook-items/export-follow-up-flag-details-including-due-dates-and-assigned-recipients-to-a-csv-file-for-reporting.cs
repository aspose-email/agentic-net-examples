using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if they appear to be dummy values
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (exchangeUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder Exchange credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string outputCsvPath = "FollowUpReport.csv";
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputCsvPath));
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

            // Connect to Exchange server
            using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
            {
                try
                {
                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                    if (messages == null)
                    {
                        Console.Error.WriteLine("No messages retrieved from the Inbox.");
                        return;
                    }

                    // Open CSV for writing
                    using (StreamWriter writer = new StreamWriter(outputCsvPath, false, Encoding.UTF8))
                    {
                        // Write CSV header
                        writer.WriteLine("Subject,FlagRequest,DueDate,RecipientsFlagRequest,RecipientsReminderTime");

                        // Process each message
                        foreach (ExchangeMessageInfo msgInfo in messages)
                        {
                            try
                            {
                                // Fetch the full MAPI message
                                MapiMessage mapiMsg = client.FetchMapiMessage(msgInfo.UniqueUri);
                                if (mapiMsg == null)
                                    continue;

                                // Get follow‑up options
                                FollowUpOptions options = FollowUpManager.GetOptions(mapiMsg);
                                if (options == null)
                                    continue;

                                // Prepare CSV fields (escape commas)
                                string subject = EscapeCsv(mapiMsg.Subject);
                                string flagRequest = EscapeCsv(options.FlagRequest);
                                string dueDate = options.DueDate != DateTime.MinValue ? options.DueDate.ToString("o") : "";
                                string recipientsFlag = EscapeCsv(options.RecipientsFlagRequest);
                                string recipientsReminder = options.RecipientsReminderTime != DateTime.MinValue ? options.RecipientsReminderTime.ToString("o") : "";

                                // Write line
                                writer.WriteLine($"{subject},{flagRequest},{dueDate},{recipientsFlag},{recipientsReminder}");
                            }
                            catch (Exception innerEx)
                            {
                                Console.Error.WriteLine($"Error processing message UID {msgInfo.UniqueUri}: {innerEx.Message}");
                                // Continue with next message
                            }
                        }
                    }

                    Console.WriteLine($"Follow‑up report exported to: {outputCsvPath}");
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to escape CSV fields containing commas or quotes
    private static string EscapeCsv(string field)
    {
        if (string.IsNullOrEmpty(field))
            return "";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            string escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        return field;
    }
}
