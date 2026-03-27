using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file
                string msgFilePath = "sample.msg";

                // Verify that the MSG file exists
                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgFilePath}");
                    return;
                }

                // Load the MSG file into a MapiMessage
                try
                {
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                    {
                        // Convert the MapiMessage to a MailMessage
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                        {
                            // Prepare token provider for Microsoft Graph authentication
                            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                                "clientId",
                                "clientSecret",
                                "refreshToken");

                            // Tenant identifier (replace with actual tenant ID)
                            string tenantId = "tenant-id";

                            // Create the Graph client
                            try
                            {
                                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                                {
                                    // Folder identifier where the message will be created (e.g., Drafts)
                                    string folderId = "Drafts";

                                    // Create the message in the specified folder
                                    graphClient.CreateMessage(folderId, mailMessage);
                                    Console.WriteLine("Message created successfully in Graph.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Graph client error: {ex.Message}");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load or convert MSG file: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}