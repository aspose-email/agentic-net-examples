using System;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";
            string notebookId = "your-notebook-id";
            string outputPath = "Notebook.json";

            // Guard against placeholder values to avoid real network calls during CI.
            if (clientId.StartsWith("your-") ||
                clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") ||
                tenantId.StartsWith("your-") ||
                notebookId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials or IDs detected – skipping Graph call.");
                return;
            }

            // Create token provider (3‑argument overload).
            Aspose.Email.Clients.TokenProvider tokenProvider =
                Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Retrieve the notebook.
                Aspose.Email.Clients.Graph.Notebook notebook = client.FetchNotebook(notebookId);

                // Serialize notebook to JSON.
                string json = JsonSerializer.Serialize(notebook, new JsonSerializerOptions { WriteIndented = true });

                // Ensure output directory exists.
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write JSON to file with error handling.
                try
                {
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Notebook saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write notebook file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
