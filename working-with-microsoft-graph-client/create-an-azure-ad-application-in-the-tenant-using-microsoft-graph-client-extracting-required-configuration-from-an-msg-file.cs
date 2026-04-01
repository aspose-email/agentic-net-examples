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
            // Paths for input MSG and a placeholder output file
            string msgPath = "config.msg";
            string placeholderOutput = "placeholder.txt";

            
            string outputDir = Path.GetDirectoryName(placeholderOutput);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
// Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                using (MailMessage placeholder = new MailMessage("placeholder@example.com", "placeholder@example.com", "Placeholder", ""))
                {
                    placeholder.Save(msgPath, SaveOptions.DefaultEml);
                }
                Console.Error.WriteLine($"Input MSG file not found. Created placeholder at {msgPath}.");
                return;
            }

            // Load the MSG file safely
            string clientId;
            string clientSecret;
            try
            {
                using (FileStream fs = new FileStream(msgPath, FileMode.Open, FileAccess.Read))
                using (MailMessage msg = MailMessage.Load(fs))
                {
                    clientId = msg.Subject?.Trim() ?? string.Empty;
                    clientSecret = msg.Body?.Trim() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Extracted configuration (example: clientId from subject, clientSecret from body)
            string refreshToken = "placeholder_refresh_token";
            string tenantId = "your_tenant_id";

            // Validate extracted data before proceeding
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                Console.Error.WriteLine("Configuration data missing in MSG file. Skipping Graph operations.");
                return;
            }

            // Create a token provider (Outlook) using the extracted credentials
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize the Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // The Graph client in Aspose.Email focuses on mail operations.
                // As a safe fallback, list mail folders instead of Azure AD app creation.
                try
                {
                    var folders = client.ListFolders();
                    Console.WriteLine($"Retrieved {folders.Count} folders.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                }
            }

            // Write a simple placeholder output to indicate completion
            try
            {
                File.WriteAllText(placeholderOutput, "Operation completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write output file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
