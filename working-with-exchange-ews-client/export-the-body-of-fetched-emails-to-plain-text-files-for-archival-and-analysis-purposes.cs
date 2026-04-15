using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Define the folder to export from (Inbox)
                string folderUri = client.MailboxInfo.InboxUri;

                // Ensure the output directory exists
                string outputDir = "ExportedEmails";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

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

                // List messages in the folder
                try
                {
                    foreach (ExchangeMessageInfo messageInfo in client.ListMessages(folderUri))
                    {
                        // Fetch the full mail message
                        MailMessage mailMessage;
                        try
                        {
                            mailMessage = client.FetchMessage(messageInfo.UniqueUri);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {messageInfo.UniqueUri}: {fetchEx.Message}");
                            continue;
                        }

                        // Prepare a safe file name using the subject
                        string safeSubject = string.IsNullOrEmpty(mailMessage.Subject) ? "NoSubject" : mailMessage.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }
                        string filePath = Path.Combine(outputDir, $"{safeSubject}.txt");

                        // Write the body to a text file
                        try
                        {
                            File.WriteAllText(filePath, mailMessage.Body);
                            Console.WriteLine($"Exported: {filePath}");
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write file {filePath}: {writeEx.Message}");
                        }
                    }
                }
                catch (Exception listEx)
                {
                    Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
