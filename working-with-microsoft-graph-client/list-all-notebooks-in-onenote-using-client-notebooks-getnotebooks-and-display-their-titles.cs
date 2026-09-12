using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphOneNote
{
    class Program
    {
        static void Main()
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string tenantId = "YOUR_TENANT_ID";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against unfilled placeholders.
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                tenantId.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace placeholder credential values with actual data.");
                return;
            }

            try
            {
                // Create a token provider for Microsoft Graph (Outlook provider).
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Obtain a Graph client instance.
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com/v1.0"))
                {
                    // Retrieve the collection of OneNote notebooks.
                    NotebookCollection notebooks = client.ListNotebooks();

                    // Display notebook titles.
                    foreach (Notebook notebook in notebooks)
                    {
                        // The Notebook class exposes a DisplayName property (title of the notebook).
                        Console.WriteLine(notebook.DisplayName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
