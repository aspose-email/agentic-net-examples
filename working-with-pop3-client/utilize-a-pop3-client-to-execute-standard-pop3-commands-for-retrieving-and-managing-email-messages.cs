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
            // POP3 server connection parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.None;

            // Initialize and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // Validate credentials
                    client.ValidateCredentials();

                    // Get mailbox status
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Message Count: {mailboxInfo.MessageCount}");
                    Console.WriteLine($"Occupied Size: {mailboxInfo.OccupiedSize} bytes");

                    // List messages (default fields)
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        Console.WriteLine($"Seq#: {info.SequenceNumber} | Subject: {info.Subject} | Size: {info.Size} bytes");
                    }

                    // If there is at least one message, fetch and save it
                    if (messageInfos.Count > 0)
                    {
                        Pop3MessageInfo firstInfo = messageInfos[0];
                        MailMessage message = client.FetchMessage(firstInfo.SequenceNumber);

                        // Prepare file path for saving the message
                        string saveDirectory = Path.Combine(Environment.CurrentDirectory, "SavedMessages");
                        string savePath = Path.Combine(saveDirectory, $"Message_{firstInfo.SequenceNumber}.eml");

                        // Ensure the directory exists
                        if (!Directory.Exists(saveDirectory))
                        {
                            Directory.CreateDirectory(saveDirectory);
                        }

                        // Save the message to a file
                        try
                        {
                            message.Save(savePath, SaveOptions.DefaultEml);
                            Console.WriteLine($"Message saved to: {savePath}");
                        }
                        catch (Exception fileEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {fileEx.Message}");
                        }

                        // Optionally delete the message from the server
                        try
                        {
                            client.DeleteMessage(firstInfo.SequenceNumber);
                            client.CommitDeletes();
                            Console.WriteLine("Message deleted from server.");
                        }
                        catch (Exception deleteEx)
                        {
                            Console.Error.WriteLine($"Failed to delete message: {deleteEx.Message}");
                        }
                    }
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
