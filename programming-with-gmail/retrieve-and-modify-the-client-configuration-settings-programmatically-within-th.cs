using System;
using System.Net;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail client with dummy OAuth credentials
            IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com");

            using (gmailClient)
            {
                // Retrieve current configuration
                string accessToken = gmailClient.AccessToken;
                string defaultEmail = gmailClient.DefaultEmail;
                int timeout = gmailClient.Timeout;

                Console.WriteLine($"Access Token: {accessToken}");
                Console.WriteLine($"Default Email: {defaultEmail}");
                Console.WriteLine($"Timeout (ms): {timeout}");

                // Modify configuration: increase timeout and set a proxy
                gmailClient.Timeout = 200000; // 200 seconds

                WebProxy proxy = new WebProxy("http://proxy.example.com:8080");
                gmailClient.Proxy = proxy;

                Console.WriteLine("Modified Timeout and Proxy settings applied.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
