using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Connect to the server (ValidateCredentials also attempts connection)
                    client.ValidateCredentials();

                    // Retrieve the list of messages
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found on the POP3 server.");
                        return;
                    }

                    // Select the first message (you can change the selection logic as needed)
                    int messageIndex = messages[0].SequenceNumber;

                    // Prepare output directory and file path
                    string outputDirectory = "DownloadedEmails";
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    string outputFilePath = Path.Combine(outputDirectory, $"Email_{messageIndex}.eml");

                    // Save the selected message to a local file
                    try
                    {
                        client.SaveMessage(messageIndex, outputFilePath);
                        Console.WriteLine($"Message {messageIndex} saved to '{outputFilePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message {messageIndex}: {ex.Message}");
                        return;
                    }

                    // Delete the message from the server
                    try
                    {
                        client.DeleteMessage(messageIndex);
                        client.CommitDeletes();
                        Console.WriteLine($"Message {messageIndex} deleted from the server.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete message {messageIndex}: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
