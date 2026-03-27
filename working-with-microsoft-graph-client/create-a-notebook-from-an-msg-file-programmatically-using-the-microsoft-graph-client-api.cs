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
            // Paths and credentials (replace with real values)
            string msgFilePath = "sample.msg";
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Guard file existence
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MailMessage
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Create Outlook token provider
            TokenProvider tokenProvider;
            try
            {
                tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            // Initialize Graph client
            IGraphClient client;
            try
            {
                client = GraphClient.GetClient(tokenProvider, tenantId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                return;
            }

            // Ensure client is disposed
            using (client)
            {
                // Build a simple Notebook using the mail subject as its name
                Notebook notebook = new Notebook
                {
                    DisplayName = mailMessage.Subject ?? "Untitled Notebook"
                };

                // Create the notebook in the user's OneNote library
                try
                {
                    client.CreateNotebook(notebook);
                    Console.WriteLine("Notebook created successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create notebook: {ex.Message}");
                }
            }

            // Dispose the loaded MailMessage
            mailMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
