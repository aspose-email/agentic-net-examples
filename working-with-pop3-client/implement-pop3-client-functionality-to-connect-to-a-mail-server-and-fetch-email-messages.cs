using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Pop3;

namespace Pop3Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Server configuration
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";
                string saveDirectory = "SavedEmails";

                // Ensure the save directory exists
                try
                {
                    if (!Directory.Exists(saveDirectory))
                    {
                        Directory.CreateDirectory(saveDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{saveDirectory}': {dirEx.Message}");
                    return;
                }

                // Initialize POP3 client
                try
                {
                    using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                    {
                        // Get mailbox information
                        Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine($"Message count: {mailboxInfo.MessageCount}, Size: {mailboxInfo.OccupiedSize} bytes");

                        // List messages
                        Pop3MessageInfoCollection messages = client.ListMessages();
                        foreach (Pop3MessageInfo info in messages)
                        {
                            Console.WriteLine($"Fetching message #{info.SequenceNumber}: {info.Subject}");

                            // Fetch the full message
                            using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"Date: {message.Date}");

                                // Save the message to a file
                                string filePath = Path.Combine(saveDirectory, $"Email_{info.SequenceNumber}.eml");
                                try
                                {
                                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                    {
                                        client.SaveMessage(info.SequenceNumber, fileStream);
                                    }
                                    Console.WriteLine($"Saved to {filePath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message #{info.SequenceNumber}: {saveEx.Message}");
                                }
                            }
                        }
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
}
