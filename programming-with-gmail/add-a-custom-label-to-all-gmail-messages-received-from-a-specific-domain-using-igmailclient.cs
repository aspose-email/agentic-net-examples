using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholders are detected.
            if (accessToken == "YOUR_ACCESS_TOKEN" || defaultEmail == "user@example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            using (gmailClient)
            {
                // Domain to filter messages from.
                string targetDomain = "example.org";
                // Custom label to add.
                string customLabel = "MyCustomLabel";

                List<GmailMessageInfo> messages;
                try
                {
                    messages = gmailClient.ListMessages();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                foreach (GmailMessageInfo info in messages)
                {
                    // Fetch full message to inspect headers.
                    MailMessage message;
                    try
                    {
                        message = gmailClient.FetchMessage(info.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {info.Id}: {ex.Message}");
                        continue;
                    }

                    // Check if the sender's email ends with the target domain.
                    string fromAddress = message.From?.Address ?? string.Empty;
                    if (fromAddress.EndsWith("@" + targetDomain, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            // Append the same message with the custom label.
                            // This creates a copy of the message labeled accordingly.
                            gmailClient.AppendMessage(message, customLabel);
                            Console.WriteLine($"Added label '{customLabel}' to message ID {info.Id}.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add label to message {info.Id}: {ex.Message}");
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
