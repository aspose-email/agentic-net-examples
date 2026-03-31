using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph call.");
                return;
            }

            // Create token provider for Outlook.
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // ID of the message that contains the attachment.
                string messageId = "MESSAGE_ID";

                // List attachments of the message.
                MapiAttachmentCollection attachments;
                try
                {
                    attachments = client.ListAttachments(messageId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list attachments: {ex.Message}");
                    return;
                }

                // Directory to save MSG attachments.
                string outputDir = Path.Combine(Environment.CurrentDirectory, "Attachments");
                try
                {
                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }

                foreach (MapiAttachment attachment in attachments)
                {
                    // Process only MSG format attachments.
                    if (Path.GetExtension(attachment.FileName).Equals(".msg", StringComparison.OrdinalIgnoreCase))
                    {
                        string outputPath = Path.Combine(outputDir, attachment.FileName);
                        try
                        {
                            // Save the attachment directly; no need to call FetchAttachment.
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved MSG attachment to: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
