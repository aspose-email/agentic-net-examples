using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "username";
                string password = "password";

                // Guard against executing real network calls with placeholder credentials
                if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder IMAP settings detected. Skipping live connection.");
                    return;
                }

                // Create and use the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Select the INBOX folder
                        client.SelectFolder("INBOX");

                        // Retrieve the list of messages in the folder
                        ImapMessageInfoCollection messages = client.ListMessages();

                        if (messages != null && messages.Count > 0)
                        {
                            // Take the first message as an example
                            ImapMessageInfo firstMessage = messages[0];

                            // Combine desired flags: Read, Answered, Flagged (used as importance)
                            ImapMessageFlags flagsToAdd = ImapMessageFlags.IsRead |
                                                          ImapMessageFlags.Answered |
                                                          ImapMessageFlags.Flagged;

                            // Apply the flags to the message using its unique identifier
                            client.AddMessageFlags(firstMessage.UniqueId, flagsToAdd);
                            Console.WriteLine($"Flags applied to message UID: {firstMessage.UniqueId}");
                        }
                        else
                        {
                            Console.WriteLine("No messages found in the INBOX folder.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during IMAP operations
                        Console.Error.WriteLine($"IMAP operation error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
