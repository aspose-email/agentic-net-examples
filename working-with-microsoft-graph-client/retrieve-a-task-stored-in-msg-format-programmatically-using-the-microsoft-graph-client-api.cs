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
            // Initialize token provider (dummy credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId", "clientSecret", "refreshToken");

            // Create Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                // ID of the MSG task to retrieve
                string messageId = "MESSAGE_ID";

                // Fetch the message as a MapiMessage (disposable)
                using (MapiMessage taskMessage = client.FetchMessage(messageId))
                {
                    // Verify that the fetched item is a task
                    if (!string.IsNullOrEmpty(taskMessage.MessageClass) && taskMessage.MessageClass.StartsWith("IPM.Task"))
                    {
                        Console.WriteLine("Task Subject: " + taskMessage.Subject);

                        // Save the task to a local MSG file
                        string outputPath = "task.msg";

                        // Ensure the target directory exists
                        string directory = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        taskMessage.Save(outputPath);
                        Console.WriteLine("Task saved to: " + outputPath);
                    }
                    else
                    {
                        Console.WriteLine("The fetched item is not a task.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
