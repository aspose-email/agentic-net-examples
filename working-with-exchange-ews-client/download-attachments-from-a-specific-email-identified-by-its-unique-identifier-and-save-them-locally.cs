using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mime;

namespace DownloadAttachmentsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange server URI and credentials
                string serverUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Unique identifier of the email message
                string messageId = "unique-message-id";

                // Folder where attachments will be saved
                string outputFolder = "Attachments";


                // Skip external calls when placeholder credentials are used
                if (serverUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure the output folder exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Create the Exchange client
                using (IEWSClient client = EWSClient.GetEWSClient(serverUri, new NetworkCredential(username, password)))
                {
                    // Fetch the email message by its unique identifier
                    MailMessage message = client.FetchMessage(messageId);

                    // Process each attachment
                    if (message.Attachments != null && message.Attachments.Count > 0)
                    {
                        foreach (Attachment attachment in message.Attachments)
                        {
                            try
                            {
                                string filePath = Path.Combine(outputFolder, attachment.Name);
                                attachment.Save(filePath);
                                Console.WriteLine($"Saved attachment: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No attachments found in the specified message.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
