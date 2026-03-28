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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG file and extract a category name (using the subject as an example)
            string categoryName;
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                categoryName = string.IsNullOrWhiteSpace(message.Subject) ? "DefaultCategory" : message.Subject;
            }

            // Prepare the token provider for Microsoft Graph (replace placeholders with real values)
            Aspose.Email.Clients.ITokenProvider tokenProvider =
                Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "YOUR_CLIENT_ID",
                    "YOUR_CLIENT_SECRET",
                    "YOUR_REFRESH_TOKEN");

            // Create the Graph client for the target user (replace with actual user email)
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "user@example.com"))
            {
                // Create a new Outlook category using the extracted name and a preset color
                OutlookCategory createdCategory = client.CreateCategory(
                    categoryName,
                    CategoryPreset.Preset0); // Choose an appropriate preset

                Console.WriteLine($"Created category: {createdCategory.DisplayName} (Id: {createdCategory.Id})");

                // List all categories for verification
                var allCategories = client.ListCategories();
                Console.WriteLine("Current categories:");
                foreach (OutlookCategory cat in allCategories)
                {
                    Console.WriteLine($"- {cat.DisplayName} (Color: {cat.Color})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
