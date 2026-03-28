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

            // Verify the input file exists
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


            // Load the MSG file into a MailMessage (disposed after use)
            using (FileStream msgStream = File.OpenRead(msgPath))
            {
                using (MailMessage message = MailMessage.Load(msgStream))
                {
                    // Message loaded – you can access its properties if needed
                    // For this example we only need to ensure the file is read successfully
                }
            }

            // Create a token provider for Outlook (3‑argument overload)
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Initialize the Graph client (disposable)
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, ""))
            {
                // Define a new OneNote notebook
                Notebook notebook = new Notebook
                {
                    DisplayName = "ImportedNotebook"
                };

                // Create the notebook in the user's OneNote library
                Notebook createdNotebook = client.CreateNotebook(notebook);

                // Output the identifier of the created notebook
                Console.WriteLine($"Notebook created with ID: {createdNotebook.Id}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
