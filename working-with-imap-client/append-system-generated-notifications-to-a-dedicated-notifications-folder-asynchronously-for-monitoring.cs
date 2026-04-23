using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip execution in CI environments
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP server detected; skipping execution.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, 993, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Callback for monitoring events
                ImapMonitoringEventHandler onChange = (object sender, ImapMonitoringEventArgs e) =>
                {
                    foreach (var msgInfo in e.NewMessages)
                    {
                        // Build a simple notification message
                        MailMessage notification = new MailMessage(
                            "no-reply@mydomain.com",
                            username,
                            "New message arrived",
                            $"A new message with subject '{msgInfo.Subject}' arrived in folder {e.FolderName}."
                        );

                        try
                        {
                            // Append the notification to the "Notifications" folder
                            client.AppendMessageAsync("Notifications", notification)
                                  .GetAwaiter()
                                  .GetResult();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to append notification: {ex.Message}");
                        }
                    }
                };

                // Callback for monitoring errors
                ImapMonitoringErrorEventHandler onError = (object sender, ImapMonitoringErrorEventArgs e) =>
                {
                    Console.Error.WriteLine($"Monitoring error: {e.Error.Message}");
                };

                // Start asynchronous monitoring of the INBOX folder
                await client.StartMonitoringAsync(onChange, onError, "INBOX");

                Console.WriteLine("Monitoring started. Press Enter to stop.");
                Console.ReadLine();

                // Stop monitoring before exiting
                await client.StopMonitoringAsync("INBOX");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
