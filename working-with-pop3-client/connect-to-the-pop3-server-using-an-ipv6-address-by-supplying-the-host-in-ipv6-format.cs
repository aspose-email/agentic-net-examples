using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // IPv6 host address (replace with actual server address)
            string ipv6Host = "2001:db8::1";
            string username = "username";
            string password = "password";
            int port = 110;
            SecurityOptions security = SecurityOptions.Auto;

            // Guard against placeholder credentials/host
            if (ipv6Host.Contains("example") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(ipv6Host, port, username, password, security))
            {
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("Connected and authenticated successfully.");

                    // List messages in the mailbox
                    Pop3MessageInfoCollection messages = client.ListMessages();
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                    }

                    // Save the first message to a local file (if any)
                    if (messages.Count > 0)
                    {
                        int sequenceNumber = messages[0].SequenceNumber;
                        string outputPath = "message.eml";

                        // Ensure the output directory exists
                        string directory = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        try
                        {
                            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                client.SaveMessage(sequenceNumber, fileStream);
                            }
                            Console.WriteLine($"Message saved to {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
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
