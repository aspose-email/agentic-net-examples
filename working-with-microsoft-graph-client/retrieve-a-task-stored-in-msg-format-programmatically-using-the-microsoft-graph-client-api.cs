using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

namespace RetrieveTaskFromGraph
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials and identifiers
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string messageId = "messageId";
                string outputPath = "task.msg";

                // Guard against placeholder credentials to avoid external calls
                if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create token provider
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, null))
                {
                    // Fetch the MSG task as a MapiMessage
                    using (MapiMessage taskMessage = client.FetchMessage(messageId))
                    {
                        // Save the message to a file
                        taskMessage.Save(outputPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
