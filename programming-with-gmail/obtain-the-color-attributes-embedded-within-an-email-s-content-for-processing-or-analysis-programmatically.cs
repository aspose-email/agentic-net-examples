using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace EmailColorExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values when available
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Skip external call when placeholder credentials are used
                if (clientId == "clientId" && clientSecret == "clientSecret" && refreshToken == "refreshToken")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                    return;
                }

                // Create Gmail client
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    // Retrieve color information from the Gmail account
                    ColorsInfo colorsInfo = gmailClient.GetColors();

                    // Output retrieved color attributes (example)
                    Console.WriteLine("Colors information retrieved:");
                    Console.WriteLine(colorsInfo);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
