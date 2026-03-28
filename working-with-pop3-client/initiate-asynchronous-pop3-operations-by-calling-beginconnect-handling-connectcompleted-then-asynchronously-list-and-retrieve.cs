using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // POP3 server settings (replace with real values)
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Output directory for saved messages
            string outputDir = Path.Combine(Environment.CurrentDirectory, "Pop3Messages");
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Create POP3 client and ensure it is disposed properly
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // Asynchronously list messages
                    Pop3MessageInfoCollection messages = await client.ListMessagesAsync();

                    Console.WriteLine($"Total messages: {messages.Count}");

                    if (messages.Count == 0)
                    {
                        Console.WriteLine("No messages to retrieve.");
                        return;
                    }

                    // Retrieve the first message asynchronously
                    Pop3MessageInfo firstInfo = messages[0];
                    using (MailMessage message = await client.FetchMessageAsync(firstInfo.SequenceNumber))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");

                        // Save the message to a file asynchronously
                        string filePath = Path.Combine(outputDir, $"Message_{firstInfo.SequenceNumber}.eml");
                        try
                        {
                            // Ensure the file can be created
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                await client.SaveMessageAsync(firstInfo.SequenceNumber, fs);
                            }
                            Console.WriteLine($"Message saved to: {filePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                        }
                    }
                }
                catch (Pop3Exception popEx)
                {
                    Console.Error.WriteLine($"POP3 error: {popEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection error: {ex.Message}");
                }
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Unexpected error: {outerEx.Message}");
        }
    }
}
