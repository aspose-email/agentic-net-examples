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
            // Token provider parameters (replace with real values)
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Create Outlook token provider
            TokenProvider tokenProvider;
            try
            {
                tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating token provider: {ex.Message}");
                return;
            }

            // Initialize Graph client
            IGraphClient client;
            try
            {
                client = Aspose.Email.Clients.Graph.GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating Graph client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Create a mail message and simulate DB modifications
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Original Subject";
                message.Body = "Original body content.";

                // Simulated persistence: update subject and body
                message.Subject = "Updated Subject";
                message.Body = "Updated body after database persistence.";

                // Prepare output directory and file path
                string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
                string outputPath = Path.Combine(outputDir, "UpdatedMessage.msg");

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error ensuring output directory: {ex.Message}");
                    return;
                }

                // Save the message as MSG
                try
                {
                    using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        message.Save(fs, SaveOptions.DefaultMsg);
                    }
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
