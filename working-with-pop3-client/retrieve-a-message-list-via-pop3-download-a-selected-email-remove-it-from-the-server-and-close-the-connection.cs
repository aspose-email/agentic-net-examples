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
            // POP3 server connection settings
            string host = "pop3.example.com";
            int port = 110; // default POP3 port
            string username = "user@example.com";
            string password = "password";

            // Path to save the downloaded message
            string savePath = "downloaded.eml";

            // Ensure the directory for the save path exists
            try
            {
                string directory = Path.GetDirectoryName(savePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare directory for saving message: {dirEx.Message}");
                return;
            }

            // Create and use the POP3 client
            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // List messages on the server
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found on the POP3 server.");
                        return;
                    }

                    // Display basic info for each message
                    Console.WriteLine("Messages on server:");
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Seq: {info.SequenceNumber}, Subject: {info.Subject}");
                    }

                    // Select the first message (or any desired one)
                    Pop3MessageInfo selectedInfo = messages[0];
                    int seqNumber = selectedInfo.SequenceNumber;

                    // Fetch the full message
                    MailMessage message = client.FetchMessage(seqNumber);
                    Console.WriteLine($"Fetched message subject: {message.Subject}");

                    // Save the message to a file
                    try
                    {
                        client.SaveMessage(seqNumber, savePath);
                        Console.WriteLine($"Message saved to: {savePath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                    }

                    // Delete the message from the server
                    client.DeleteMessage(seqNumber);
                    client.CommitDeletes();
                    Console.WriteLine("Message deleted from server.");
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
