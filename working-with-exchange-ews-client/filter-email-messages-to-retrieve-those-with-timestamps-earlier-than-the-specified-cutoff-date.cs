using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Define cutoff date
            DateTime cutoffDate = new DateTime(2023, 1, 1);

            // Initialize Gmail client with placeholder credentials
            IGmailClient client;
            try
            {
                client = GmailClient.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken",
                    "user@example.com");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed after use
            using (client)
            {
                // Retrieve list of message infos
                List<GmailMessageInfo> messageInfos;
                try
                {
                    messageInfos = client.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Iterate through messages and filter by date
                foreach (GmailMessageInfo info in messageInfos)
                {
                    MailMessage fullMessage;
                    try
                    {
                        fullMessage = client.FetchMessage(info.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {info.Id}: {ex.Message}");
                        continue;
                    }

                    using (fullMessage)
                    {
                        if (fullMessage.Date < cutoffDate)
                        {
                            Console.WriteLine($"Subject: {fullMessage.Subject}");
                            Console.WriteLine($"Date: {fullMessage.Date}");
                            Console.WriteLine();
                        }
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
