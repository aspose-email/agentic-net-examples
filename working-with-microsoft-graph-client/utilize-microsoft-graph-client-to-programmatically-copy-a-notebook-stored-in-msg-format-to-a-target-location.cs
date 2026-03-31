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
            // Placeholder credentials – replace with real values or skip execution if placeholders are used.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                string.IsNullOrWhiteSpace(tenantId) ||
                clientId.StartsWith("your-") ||
                clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") ||
                tenantId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operation.");
                return;
            }

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            string msgPath = "notebook.msg";
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Notebook", "Placeholder notebook content"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file (optional – shown for completeness).
            try
            {
                using (MapiMessage _ = MapiMessage.Load(msgPath))
                {
                    // No further processing needed for this example.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Create token provider using the verified Outlook overload (3 arguments).
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Initialize Graph client.
            IGraphClient client = null;
            try
            {
                client = GraphClient.GetClient(tokenProvider, tenantId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Graph client: {ex.Message}");
                return;
            }

            // Use the client to copy the notebook.
            try
            {
                // In a real scenario, itemId would be obtained from the source notebook.
                string itemId = "source-notebook-item-id";
                string groupId = "target-group-id"; // Use empty string if not copying to a group.
                string renameAs = "CopiedNotebook";

                string operationLocation = client.CopyNotebook(itemId, groupId, renameAs);
                Console.WriteLine($"CopyNotebook operation started. Operation-Location: {operationLocation}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CopyNotebook failed: {ex.Message}");
            }
            finally
            {
                client?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
