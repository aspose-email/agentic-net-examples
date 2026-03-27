using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

namespace AsposeEmailGraphAttachmentExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder authentication and message identifiers
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";
                string messageId = "MESSAGE_ID";

                // Ensure output directory exists
                string outputDirectory = "Attachments";
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                    return;
                }

                // Create token provider
                Aspose.Email.Clients.ITokenProvider tokenProvider;
                try
                {
                    tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
                }
                catch (Exception tokenEx)
                {
                    Console.Error.WriteLine($"Failed to create token provider: {tokenEx.Message}");
                    return;
                }

                // Create Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // List attachments for the specified message
                    MapiAttachmentCollection attachmentCollection;
                    try
                    {
                        attachmentCollection = graphClient.ListAttachments(messageId);
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Failed to list attachments: {listEx.Message}");
                        return;
                    }

                    foreach (MapiAttachment attachment in attachmentCollection)
                    {
                        // Retrieve the attachment using its ItemId
                        MapiAttachment fetchedAttachment;
                        try
                        {
                            fetchedAttachment = graphClient.FetchAttachment(attachment.ItemId);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch attachment '{attachment.FileName}': {fetchEx.Message}");
                            continue;
                        }

                        // Determine file name and path
                        string fileName = string.IsNullOrEmpty(fetchedAttachment.FileName) ? "attachment.dat" : fetchedAttachment.FileName;
                        string filePath = Path.Combine(outputDirectory, fileName);

                        // Save the attachment to disk
                        try
                        {
                            fetchedAttachment.Save(filePath);
                            Console.WriteLine($"Attachment saved: {filePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{fileName}': {saveEx.Message}");
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
}