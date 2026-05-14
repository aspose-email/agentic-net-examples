using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 server detected. Skipping execution.");
                return;
            }

            // Output directory for saved messages
            string outputDir = "SavedMessages";

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

            // Initialize POP3 client with multiconnection mode enabled
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.UseMultiConnection = MultiConnectionMode.Enable;

                    // Validate credentials (wrapped in its own try/catch)
                    try
                    {
                        client.ValidateCredentials();
                    }
                    catch (Exception credEx)
                    {
                        Console.Error.WriteLine($"Credential validation failed: {credEx.Message}");
                        return;
                    }

                    // List messages on the server
                    Pop3MessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages();
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                        return;
                    }

                    // Iterate through each message info
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        int sequenceNumber = info.SequenceNumber;

                        // Fetch the full message
                        MailMessage message;
                        try
                        {
                            message = client.FetchMessage(sequenceNumber);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message #{sequenceNumber}: {fetchEx.Message}");
                            continue;
                        }

                        // Save the message to a file
                        using (message)
                        {
                            string filePath = Path.Combine(outputDir, $"Message_{sequenceNumber}.eml");
                            try
                            {
                                message.Save(filePath);
                                Console.WriteLine($"Saved message #{sequenceNumber} to {filePath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message #{sequenceNumber}: {saveEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
