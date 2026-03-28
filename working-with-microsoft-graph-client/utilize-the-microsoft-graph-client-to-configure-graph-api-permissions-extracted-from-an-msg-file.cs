using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Subject", "Body"))
                {
                    placeholder.Save(msgPath);
                }
                Console.Error.WriteLine($"Input file not found. Created placeholder at {msgPath}.");
                return;
            }

            // Load the MSG file.
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                string senderEmail = message.From?.Address ?? "unknown@example.com";

                // Initialize token provider for Outlook (dummy credentials).
                Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId", "clientSecret", "refreshToken");

                // Create Graph client.
                IGraphClient client;
                try
                {
                    client = GraphClient.GetClient(tokenProvider, "tenantId");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                    return;
                }

                using (client)
                {
                    // Create or update a classification override for the sender.
                    try
                    {
                        client.CreateOrUpdateOverride(new MailAddress(senderEmail), ClassificationType.Focused);
                        Console.WriteLine("Override created/updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create/update override: {ex.Message}");
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
