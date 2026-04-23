using Aspose.Email;
using System;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                return;
            }

            // Create Gmail client safely.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Use the client and retrieve the color palette.
            using (gmailClient)
            {
                try
                {
                    ColorsInfo colorsInfo = gmailClient.GetColors();
                    Console.WriteLine("Gmail color palette retrieved:");
                    Console.WriteLine(colorsInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving colors: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
