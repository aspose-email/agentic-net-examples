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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Verify that the file exists
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


            // Load the MSG file into a MapiMessage instance
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Initialize a token provider for Outlook (dummy credentials)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(
                    "clientId", "clientSecret", "refreshToken");

                // Create a Graph client instance
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
                {
                    // List tasks from the default Outlook tasks folder
                    try
                    {
                        // The folder identifier for the default tasks folder is typically "tasks"
                        var tasks = client.ListTasks("tasks");
                        foreach (var task in tasks)
                        {
                            Console.WriteLine($"Task Subject: {task.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving tasks: {ex.Message}");
                    }

                    // Demonstrate accessing information from the loaded MSG file
                    Console.WriteLine($"Loaded MSG Subject: {msg.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
