using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file
            string msgPath = "task.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Prepare token provider (dummy credentials for illustration)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Initialize Graph client for the user (dummy user email)
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "user@example.com"))
            {
                try
                {
                    // Load the MSG file into a MailMessage
                    using (MailMessage mailMessage = MailMessage.Load(msgPath))
                    {
                        // Map basic properties to a MapiTask
                        MapiTask task = new MapiTask
                        {
                            Subject = mailMessage.Subject,
                            Body = mailMessage.Body
                        };

                        // Create the task in the default "Tasks" folder
                        client.CreateTask(task, "Tasks");
                        Console.WriteLine("Task created successfully in Microsoft Graph.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Graph operation: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
