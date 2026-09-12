using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    // Author: Aspose.Email example for retrieving email size via POP3
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection details (replace with real values)
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client())
                {
                    try
                    {
                        client.Host = host;
                        client.Port = port;
                        client.Username = username;
                        client.Password = password;
                        client.SecurityOptions = SecurityOptions.Auto;

                        // Optional: get total mailbox size
                        long mailboxSize = client.GetMailboxSize();
                        Console.WriteLine($"Mailbox total size: {mailboxSize} bytes");

                        // Retrieve list of messages
                        Pop3MessageInfoCollection messages = client.ListMessages();

                        if (messages.Count > 0)
                        {
                            // Get size of the first message
                            Pop3MessageInfo firstMessageInfo = messages[0];
                            long sizeInBytes = firstMessageInfo.Size;
                            Console.WriteLine($"First message size: {sizeInBytes} bytes");

                            // Conditional handling based on size
                            if (sizeInBytes > 1024 * 1024) // larger than 1 MB
                            {
                                Console.WriteLine("Message exceeds 1 MB, apply special processing.");
                            }
                            else
                            {
                                Console.WriteLine("Message size is within normal limits.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages found in the mailbox.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                        return;
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
