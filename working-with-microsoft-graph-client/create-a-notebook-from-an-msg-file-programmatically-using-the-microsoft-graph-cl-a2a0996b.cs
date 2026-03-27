using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgFilePath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Prepare a Notebook object (using the subject as the notebook name)
                Notebook notebook = new Notebook
                {
                    DisplayName = string.IsNullOrEmpty(msg.Subject) ? "Untitled Notebook" : msg.Subject
                };

                // Token provider credentials (replace with real values)
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";

                // Create token provider
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    try
                    {
                        // Create the notebook in OneNote
                        Notebook createdNotebook = graphClient.CreateNotebook(notebook);
                        Console.WriteLine($"Notebook created with ID: {createdNotebook.Id}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                        return;
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