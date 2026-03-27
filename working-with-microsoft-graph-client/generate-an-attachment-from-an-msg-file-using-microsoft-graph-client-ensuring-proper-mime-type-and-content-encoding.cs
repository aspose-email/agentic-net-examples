using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source MSG file
            string msgFilePath = @"C:\Temp\sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (mapiMessage)
            {
                // Ensure the message contains at least one attachment
                if (mapiMessage.Attachments == null || mapiMessage.Attachments.Count == 0)
                {
                    Console.Error.WriteLine("Error: No attachments found in the MSG file.");
                    return;
                }

                // Take the first attachment from the MSG file
                MapiAttachment attachment = mapiMessage.Attachments[0];

                // Placeholder values for Graph authentication
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";

                // Create a token provider (exact three‑argument overload)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Create the Graph client (returns IGraphClient which implements IDisposable)
                IGraphClient graphClient;
                try
                {
                    graphClient = Aspose.Email.Clients.Graph.GraphClient.GetClient(tokenProvider, tenantId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating Graph client: {ex.Message}");
                    return;
                }

                using (graphClient)
                {
                    // Identifier of the item (e.g., a draft message) to which the attachment will be added
                    string parentItemId = "parent-item-id";

                    // Upload the attachment to the specified item
                    MapiAttachment uploadedAttachment;
                    try
                    {
                        uploadedAttachment = graphClient.CreateAttachment(parentItemId, attachment);
                        Console.WriteLine($"Attachment uploaded. Id: {uploadedAttachment.ItemId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error uploading attachment: {ex.Message}");
                        return;
                    }

                    // Send the original MapiMessage using MIME format
                    try
                    {
                        graphClient.SendAsMime(mapiMessage);
                        Console.WriteLine("Message sent successfully using MIME format.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending message: {ex.Message}");
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