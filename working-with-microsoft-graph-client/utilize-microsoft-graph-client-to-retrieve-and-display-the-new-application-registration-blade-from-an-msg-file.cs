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

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Create a token provider for Outlook (dummy credentials)
            Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken");

            // Initialize the Graph client (dummy tenant ID)
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                try
                {
                    // Load the MSG file
                    using (MailMessage message = MailMessage.Load(msgPath))
                    {
                        // Display basic information from the MSG file
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + message.From);
                        Console.WriteLine("To: " + string.Join(", ", message.To));
                        Console.WriteLine("Body:");
                        Console.WriteLine(message.Body);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error processing MSG file: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
