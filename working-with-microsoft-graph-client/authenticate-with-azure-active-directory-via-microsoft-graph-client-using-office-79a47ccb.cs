using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file containing Office credentials
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = new MailAddress("placeholder@example.com");
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder MSG file.";
                        placeholder.Save(msgPath, SaveOptions.DefaultMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder MSG file: " + ex.Message);
                    return;
                }
            }

            // Load the MSG file and extract the user email (used as username)
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to load MSG file: " + ex.Message);
                return;
            }

            using (mailMessage)
            {
                string userEmail = mailMessage.From != null ? mailMessage.From.Address : "user@example.com";

                // Azure AD application credentials (replace with real values)
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";

                // Create the token provider for Outlook (Microsoft Graph)
                TokenProvider tokenProvider;
                try
                {
                    tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create token provider: " + ex.Message);
                    return;
                }

                // Initialize the Graph client
                IGraphClient graphClient;
                try
                {
                    graphClient = GraphClient.GetClient(tokenProvider, tenantId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to initialize Graph client: " + ex.Message);
                    return;
                }

                // Use the Graph client (example: list messages in the Inbox)
                using (graphClient)
                {
                    try
                    {
                        // The ListMessages method returns a collection of message metadata.
                        // Here we simply invoke it; detailed processing can be added as needed.
                        var messages = graphClient.ListMessages("Inbox");
                        Console.WriteLine("Successfully retrieved messages from the Inbox.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Graph operation failed: " + ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}