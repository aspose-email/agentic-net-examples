using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailMetadata
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values for actual execution
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Skip network call when placeholders are used
                bool placeholders = clientId == "clientId" ||
                                    clientSecret == "clientSecret" ||
                                    refreshToken == "refreshToken" ||
                                    defaultEmail == "user@example.com";

                if (placeholders)
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                    return;
                }

                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    try
                    {
                        List<GmailMessageInfo> messages = gmailClient.ListMessages();

                        foreach (GmailMessageInfo messageInfo in messages)
                        {
                            // Fetch the full message to access detailed metadata
                            using (MailMessage fullMessage = gmailClient.FetchMessage(messageInfo.Id))
                            {
                                Console.WriteLine("Subject: " + fullMessage.Subject);
                                Console.WriteLine("From: " + fullMessage.From);
                                Console.WriteLine("Date: " + fullMessage.Date);
                                Console.WriteLine("To: " + fullMessage.To);
                                Console.WriteLine("Message Id: " + fullMessage.MessageId);
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during Gmail operations: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
