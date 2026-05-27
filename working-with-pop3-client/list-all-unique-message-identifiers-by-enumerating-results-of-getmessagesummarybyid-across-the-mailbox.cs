using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create POP3 client with proper SecurityOptions overload.
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve the list of message infos.
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    // Enumerate each message, fetch its summary, and output the unique identifier.
                    foreach (Pop3MessageInfo info in messages)
                    {
                        // GetMessageInfo(string) returns the message info (summary) for the given UniqueId.
                        Pop3MessageInfo summary = client.GetMessageInfo(info.UniqueId);
                        Console.WriteLine(summary.UniqueId);
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
