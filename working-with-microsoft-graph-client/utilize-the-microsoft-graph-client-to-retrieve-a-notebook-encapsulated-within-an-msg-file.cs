using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth credentials – replace with real values or keep placeholders for demo.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Create a token provider for Outlook (Microsoft Graph) authentication.
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Initialize the Graph client.
            using (IGraphClient client = Aspose.Email.Clients.Graph.GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Identifier of the notebook to retrieve – replace with a real notebook ID.
                string notebookId = "notebook-id";

                // Fetch the notebook from Microsoft Graph.
                var notebook = client.FetchNotebook(notebookId);

                // Prepare the output MSG file path.
                string outputPath = Path.Combine(Environment.CurrentDirectory, "FetchedNotebook.msg");

                // Ensure the target directory exists.
                string directory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write a simple representation of the notebook to the MSG file.
                // In a real scenario you might serialize the notebook content appropriately.
                try
                {
                    File.WriteAllText(outputPath, notebook?.ToString() ?? "Notebook data is null");
                    Console.WriteLine($"Notebook saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
