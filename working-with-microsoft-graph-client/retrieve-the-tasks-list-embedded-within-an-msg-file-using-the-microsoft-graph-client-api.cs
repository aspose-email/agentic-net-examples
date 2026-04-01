using Aspose.Email.Mapi;
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
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Guard against placeholder credentials to avoid real network calls.
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-") || tenantId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph client operations.");
                return;
            }

            // Create a token provider using the verified Outlook overload (3 arguments).
            TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId, clientSecret, refreshToken);

            // Initialize the Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                // Attempt to list tasks from the default task list.
                // Adjust the folder identifier as needed for your environment.
                var tasks = client.ListTasks("default");

                Console.WriteLine("Tasks retrieved from Graph:");
                foreach (var task in tasks)
                {
                    // Task properties such as Subject may be available; using placeholder property.
                    Console.WriteLine($"- {task.Subject}");
                }
            }

            // Path to the MSG file containing embedded tasks.
            string msgPath = "sample.msg";

            // Ensure the MSG file exists before attempting to load.
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

                // Create a minimal placeholder MSG file to avoid missing asset errors.
                using (var placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "No content"))
                {
                    placeholder.Save(msgPath);
                }
                Console.WriteLine($"Placeholder MSG file created at '{msgPath}'.");
                return;
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Example: Access embedded task items if present.
                // Aspose.Email does not expose tasks directly from MSG; this is illustrative.
                Console.WriteLine($"Loaded MSG Subject: {msg.Subject}");
                // Additional processing of embedded tasks would go here.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
