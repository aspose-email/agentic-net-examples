using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // ---------- Local MSG file handling ----------
                string msgFilePath = "sample.msg";

                // Ensure the MSG file exists; create a minimal placeholder if missing
                if (!File.Exists(msgFilePath))
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder Subject", "This is a placeholder MSG file."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }

                // Load and display the local MSG file
                using (MapiMessage localMessage = MapiMessage.Load(msgFilePath))
                {
                    Console.WriteLine("Local MSG Subject: " + localMessage.Subject);
                    Console.WriteLine("Local MSG Body: " + localMessage.Body);
                }

                // ---------- Microsoft Graph client usage ----------
                // Dummy credentials – replace with real values when running the sample
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string tenantId = "tenantId";

                // Create token provider (Outlook) – 3‑argument overload
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Placeholder message ID – replace with an actual ID from your mailbox
                    string messageId = "message-id";

                    // Fetch the message from Microsoft Graph
                    MapiMessage graphMessage = graphClient.FetchMessage(messageId);

                    // Display fetched message details
                    Console.WriteLine("Graph Message Subject: " + graphMessage.Subject);
                    Console.WriteLine("Graph Message Body: " + graphMessage.Body);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
