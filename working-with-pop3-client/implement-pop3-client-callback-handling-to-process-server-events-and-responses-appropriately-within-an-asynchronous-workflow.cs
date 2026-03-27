using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3CallbackExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // POP3 server connection settings
                string host = "pop.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Ensure the output directory exists before any file operation
                string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create and configure the POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Bind local endpoint (required for some network environments)

                    // Subscribe to the OnConnect event to receive connection notifications
                    client.OnConnect += (sender, eventArgs) =>
                    {
                        Console.WriteLine("POP3 client connected to the server.");
                    };

                    // Validate credentials safely
                    try
                    {
                        client.ValidateCredentials();
                    }
                    catch (Exception credEx)
                    {
                        Console.Error.WriteLine($"Credential validation failed: {credEx.Message}");
                        return;
                    }

                    // Retrieve mailbox information asynchronously
                    Pop3MailboxInfo mailboxInfo;
                    try
                    {
                        mailboxInfo = await client.GetMailboxInfoAsync();
                    }
                    catch (Exception mbEx)
                    {
                        Console.Error.WriteLine($"Failed to get mailbox info: {mbEx.Message}");
                        return;
                    }

                    Console.WriteLine($"Total messages on server: {mailboxInfo.MessageCount}");

                    // List messages asynchronously
                    Pop3MessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = await client.ListMessagesAsync();
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                        return;
                    }

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        Console.WriteLine($"Seq:{info.SequenceNumber} Subject:{info.Subject}");
                    }

                    // If there is at least one message, fetch and save it
                    if (messageInfos.Count > 0)
                    {
                        int sequenceNumber = messageInfos[0].SequenceNumber;
                        string outputPath = Path.Combine(outputDirectory, "FirstMessage.eml");

                        try
                        {
                            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                await client.SaveMessageAsync(sequenceNumber, fileStream);
                            }
                            Console.WriteLine($"First message saved to: {outputPath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
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
}
