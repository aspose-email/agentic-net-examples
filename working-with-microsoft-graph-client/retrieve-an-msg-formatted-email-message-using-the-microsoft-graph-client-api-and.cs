using System;
using System.IO;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

namespace AsposeEmailGraphExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize token provider (dummy credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Create Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // ID of the message to retrieve (dummy value)
                    string messageId = "messageId";

                    // Fetch the MSG-formatted message
                    using (MapiMessage message = graphClient.FetchMessage(messageId))
                    {
                        // Output basic properties
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + (message.SenderEmailAddress ?? "Unknown"));
                        Console.WriteLine("Body: " + message.Body);

                        // Process attachments
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            Console.WriteLine("Attachment: " + attachment.FileName);
                            string attachmentsFolder = "Attachments";

                            try
                            {
                                // Ensure the output directory exists
                                if (!Directory.Exists(attachmentsFolder))
                                {
                                    Directory.CreateDirectory(attachmentsFolder);
                                }

                                // Save attachment to file
                                string filePath = Path.Combine(attachmentsFolder, attachment.FileName);
                                attachment.Save(filePath);
                                Console.WriteLine("Saved attachment to: " + filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("Failed to save attachment: " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}