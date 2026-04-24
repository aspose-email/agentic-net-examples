using System;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace GmailInboxScheduler
{
    class Program
    {
        // Placeholder credentials – replace with real values or keep as placeholders to skip execution.
        private const string AccessToken = "YOUR_ACCESS_TOKEN";
        private const string DefaultEmail = "your.email@example.com";

        static void Main()
        {
            try
            {
                // Set up a timer that triggers every 15 minutes.
                using (Timer timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromMinutes(15)))
                {
                    Console.WriteLine("Gmail inbox scheduler started. Press Enter to exit.");
                    Console.ReadLine(); // Keep the application running.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void Callback(object state)
        {
            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(AccessToken) || AccessToken.StartsWith("YOUR_"))
            {
                Console.WriteLine("Skipping Gmail check due to placeholder credentials.");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(AccessToken, DefaultEmail);
                ProcessInbox(gmailClient);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail client error: {ex.Message}");
            }
            finally
            {
                // Ensure the client is disposed.
                if (gmailClient != null)
                {
                    gmailClient.Dispose();
                }
            }
        }

        private static void ProcessInbox(IGmailClient gmailClient)
        {
            try
            {
                // Retrieve list of messages in the inbox.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();

                foreach (GmailMessageInfo messageInfo in messages)
                {
                    // Fetch the full message.
                    using (MailMessage mailMessage = gmailClient.FetchMessage(messageInfo.Id))
                    {
                        // Simple processing: output subject and sender.
                        Console.WriteLine($"Received email from {mailMessage.From} with subject: {mailMessage.Subject}");
                        // Additional processing logic can be placed here.
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing inbox: {ex.Message}");
            }
        }
    }
}
