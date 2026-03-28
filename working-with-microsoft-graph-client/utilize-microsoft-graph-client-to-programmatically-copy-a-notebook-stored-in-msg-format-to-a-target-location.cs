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
            // Path to the MSG file that represents the notebook
            string msgFilePath = "notebook.msg";

            // Verify the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgFilePath))
            {
                using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", ""))
                {
                    placeholder.Save(msgFilePath);
                }
            }

            // IDs required for the copy operation (replace with real values)
            string itemId = "placeholder-item-id";
            string groupId = "target-group-id";
            string renameAs = "CopiedNotebook";

            // Create a token provider using the 3‑argument Outlook overload
            TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance("clientId", "clientSecret", "refreshToken");

            // Initialize the Graph client (IDisposable)
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // Perform the notebook copy
                string operationLocation = client.CopyNotebook(itemId, groupId, renameAs);
                Console.WriteLine("Copy operation started. Operation location: " + operationLocation);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
