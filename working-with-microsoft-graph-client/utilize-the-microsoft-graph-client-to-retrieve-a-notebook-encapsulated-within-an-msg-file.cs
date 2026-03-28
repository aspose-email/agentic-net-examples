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
            // Path to the MSG file that supposedly contains a notebook reference
            string msgPath = "sample.msg";

            // Verify the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Placeholder: extract notebook identifier from the message if needed
                // For this sample we use a hard‑coded notebook Id
                string notebookId = "YOUR_NOTEBOOK_ID";

                // Create a token provider for Outlook (3‑argument overload)
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Initialize the Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // Retrieve the notebook
                    Notebook notebook = client.FetchNotebook(notebookId);

                    // Output basic notebook information
                    Console.WriteLine($"Notebook Id: {notebook.Id}");
                    Console.WriteLine($"Notebook Name: {notebook.DisplayName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
