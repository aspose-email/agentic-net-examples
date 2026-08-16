using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string host = "pop3.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping execution.");
                return;
            }

            // Create and configure POP3 client.
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Enable multi‑connection mode.
                    client.UseMultiConnection = MultiConnectionMode.Enable;
                    client.ConnectionsQuantity = 5; // Adjust as needed.

                    // Retrieve the list of message infos.
                    Pop3MessageInfoCollection messageInfos = client.ListMessagesAsync().GetAwaiter().GetResult();

                    // Parallel fetch of message headers.
                    Parallel.ForEach(messageInfos,
                        new ParallelOptions { MaxDegreeOfParallelism = client.ConnectionsQuantity },
                        messageInfo =>
                        {
                            try
                            {
                                // Fetch headers for the current message.
                                HeaderCollection headers = client.GetMessageHeaders(messageInfo.SequenceNumber);
                                // Example processing: output subject line.
                                Console.WriteLine($"Subject: {messageInfo.Subject}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to fetch headers for message #{messageInfo.SequenceNumber}: {ex.Message}");
                            }
                        });
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
