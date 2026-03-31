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
            // Placeholder POP3 server credentials
            string host = "pop.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping POP3 operations.");
                return;
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found on the POP3 server.");
                        return;
                    }

                    // Select the first message (could be replaced with user selection)
                    Pop3MessageInfo firstInfo = messages[0];
                    int sequenceNumber = firstInfo.SequenceNumber;

                    // Fetch the full message
                    using (MailMessage message = client.FetchMessage(sequenceNumber))
                    {
                        // Define output file path
                        string outputPath = "downloaded_message.eml";

                        // Ensure directory exists
                        string directory = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Save the message to a file (guarded with try/catch)
                        try
                        {
                            client.SaveMessage(sequenceNumber, outputPath);
                            Console.WriteLine($"Message saved to: {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                        }
                    }

                    // Delete the message from the server
                    client.DeleteMessage(sequenceNumber);
                    Console.WriteLine("Message deleted from the server.");
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
