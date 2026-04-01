using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values for actual execution.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Guard against placeholder credentials to avoid external calls during CI.
                if (clientId == "clientId" || clientSecret == "clientSecret" ||
                    refreshToken == "refreshToken" || defaultEmail == "user@example.com")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                    return;
                }

                // Create Gmail client instance.
                IGmailClient gmailClient = null;
                try
                {
                    gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                // Ensure the client is disposed properly.
                using (gmailClient)
                {
                    // List messages in the mailbox.
                    List<GmailMessageInfo> messages = null;
                    try
                    {
                        messages = gmailClient.ListMessages();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                        return;
                    }

                    Console.WriteLine($"Total messages: {messages.Count}");

                    // Process the first message if available.
                    if (messages.Count > 0)
                    {
                        GmailMessageInfo firstInfo = messages[0];
                        MailMessage fetchedMessage = null;
                        try
                        {
                            fetchedMessage = gmailClient.FetchMessage(firstInfo.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch message ID {firstInfo.Id}: {ex.Message}");
                        }

                        if (fetchedMessage != null)
                        {
                            Console.WriteLine($"Subject of first message: {fetchedMessage.Subject}");
                        }
                    }

                    // Compose and send a new email.
                    MailMessage newMessage = new MailMessage();
                    newMessage.From = new MailAddress(defaultEmail);
                    newMessage.To.Add(new MailAddress(defaultEmail));
                    newMessage.Subject = "Test Email from Aspose.Email Gmail Client";
                    newMessage.Body = "This is a test email sent using Aspose.Email Gmail integration.";

                    try
                    {
                        string sentMessageId = gmailClient.SendMessage(newMessage);
                        Console.WriteLine($"Message sent successfully. ID: {sentMessageId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
