using System;
using System.IO;
using System.Net;
using Aspose.Email;
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
                string msgPath = "sample.msg";

                // Verify that the MSG file exists
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {msgPath}");
                    return;
                }

                // Load the MSG file into a MailMessage object
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    // Create a token provider for public client authentication
                    Aspose.Email.Clients.ITokenProvider tokenProvider;
                    try
                    {
                        tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                            "clientId",
                            "clientSecret",
                            "refreshToken");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating token provider: {ex.Message}");
                        return;
                    }

                    // Initialize the Graph client
                    using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                    {
                        try
                        {
                            // Folder identifier (e.g., "Inbox")
                            string folderId = "Inbox";

                            // Upload the message to the specified folder
                            graphClient.CreateMessage(folderId, mailMessage);
                            Console.WriteLine("Message uploaded successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error uploading message: {ex.Message}");
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