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
            // POP3 server connection details (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate credentials (establishes connection)
                    client.ValidateCredentials();

                    // List messages
                    Aspose.Email.Clients.Pop3.Pop3MessageInfoCollection messages = client.ListMessages();

                    Console.WriteLine($"Total messages: {messages.Count}");

                    if (messages.Count > 0)
                    {
                        // Fetch the first message info
                        Aspose.Email.Clients.Pop3.Pop3MessageInfo firstInfo = messages[0];

                        // Save the first message to a file
                        string outputPath = "message.eml";
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        try
                        {
                            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                client.SaveMessage(firstInfo.SequenceNumber, fs);
                            }
                            Console.WriteLine($"Message saved to {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                        }
                    }
                }
                catch (Pop3Exception popEx)
                {
                    Console.Error.WriteLine($"POP3 error: {popEx.Message}");
                    Console.Error.WriteLine("Attempting to reconnect...");

                    // Attempt reconnection
                    try
                    {
                        client.ValidateCredentials();

                        // Retry listing messages after reconnection
                        Aspose.Email.Clients.Pop3.Pop3MessageInfoCollection retryMessages = client.ListMessages();
                        Console.WriteLine($"After reconnection, total messages: {retryMessages.Count}");
                    }
                    catch (Pop3Exception retryEx)
                    {
                        Console.Error.WriteLine($"Reconnection failed: {retryEx.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Fatal error: {outerEx.Message}");
        }
    }
}
