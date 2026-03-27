using System;
using System.IO;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file that contains the notebook identifier
                string msgFilePath = "sample.msg";

                // Verify that the MSG file exists
                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"File not found: {msgFilePath}");
                    return;
                }

                // Load the MSG file
                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    // Attempt to extract a notebook identifier from the message.
                    // For demonstration purposes, we use the Subject as a placeholder.
                    string notebookId = msg.Subject;

                    if (string.IsNullOrEmpty(notebookId))
                    {
                        Console.Error.WriteLine("Notebook identifier not found in the MSG file.");
                        return;
                    }

                    // Create a token provider (Outlook provider) with dummy credentials.
                    // Replace the placeholder strings with real values when running the sample.
                    Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                        "clientId",
                        "clientSecret",
                        "refreshToken");

                    // Initialize the Microsoft Graph client.
                    using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
                    {
                        try
                        {
                            // Retrieve the notebook using the extracted identifier.
                            Aspose.Email.Clients.Graph.Notebook notebook = graphClient.FetchNotebook(notebookId);

                            // Output notebook details.
                            Console.WriteLine($"Notebook ID: {notebook.Id}");
                            Console.WriteLine($"Notebook Display Name: {notebook.DisplayName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
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