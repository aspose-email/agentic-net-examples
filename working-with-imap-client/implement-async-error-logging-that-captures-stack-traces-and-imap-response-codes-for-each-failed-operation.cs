using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder credentials are used.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping IMAP operations.");
                return;
            }

            // Create and connect the IMAP client.
            try
            {
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Example: List the first 10 messages in the INBOX folder.
                    try
                    {
                        ImapMessageInfoCollection messages = await client.ListMessagesAsync(10);
                        Console.WriteLine($"Retrieved {messages.Count} messages.");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine("Error during ListMessagesAsync:");
                        Console.Error.WriteLine($"Message: {imapEx.Message}");
                        Console.Error.WriteLine($"Error Details: {imapEx.ErrorDetails}");
                        Console.Error.WriteLine($"Stack Trace: {imapEx.StackTrace}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Unexpected error during ListMessagesAsync:");
                        Console.Error.WriteLine($"Message: {ex.Message}");
                        Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                        return;
                    }

                    // Example: Fetch the first message if any.
                    try
                    {
                        ImapMessageInfoCollection messages = await client.ListMessagesAsync(1);
                        if (messages.Count > 0)
                        {
                            ImapMessageInfo firstInfo = messages[0];
                            MailMessage fetched = await client.FetchMessageAsync(firstInfo.UniqueId);
                            Console.WriteLine($"Fetched message subject: {fetched.Subject}");
                        }
                        else
                        {
                            Console.WriteLine("No messages to fetch.");
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine("Error during FetchMessageAsync:");
                        Console.Error.WriteLine($"Message: {imapEx.Message}");
                        Console.Error.WriteLine($"Error Details: {imapEx.ErrorDetails}");
                        Console.Error.WriteLine($"Stack Trace: {imapEx.StackTrace}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Unexpected error during FetchMessageAsync:");
                        Console.Error.WriteLine($"Message: {ex.Message}");
                        Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                        return;
                    }
                }
            }
            catch (ImapException imapEx)
            {
                Console.Error.WriteLine("Failed to create or connect IMAP client:");
                Console.Error.WriteLine($"Message: {imapEx.Message}");
                Console.Error.WriteLine($"Error Details: {imapEx.ErrorDetails}");
                Console.Error.WriteLine($"Stack Trace: {imapEx.StackTrace}");
                return;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error during client initialization:");
                Console.Error.WriteLine($"Message: {ex.Message}");
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}
