using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values as needed)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the Exchange WebDAV client inside a using block for proper disposal
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Attempt to list messages from the Inbox folder
                ExchangeMessageInfoCollection messages;
                try
                {
                    messages = client.ListMessages("Inbox");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Ensure the output directory exists
                string outputDir = Path.Combine(Environment.CurrentDirectory, "SavedEmails");
                if (!Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Iterate through each message and save it as an EML file
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Use the UniqueUri property to identify the message
                    string messageUri = info.UniqueUri;
                    if (string.IsNullOrEmpty(messageUri))
                    {
                        Console.Error.WriteLine("Message URI is null or empty, skipping.");
                        continue;
                    }

                    // Build a safe file name using the message subject (fallback to GUID)
                    string safeSubject = string.IsNullOrWhiteSpace(info.Subject) ? Guid.NewGuid().ToString() : info.Subject;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(c, '_');
                    }
                    string emlPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                    // Save the message to the file system
                    try
                    {
                        client.SaveMessage(messageUri, emlPath);
                        Console.WriteLine($"Saved message to: {emlPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message '{info.Subject}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
