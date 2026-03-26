using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string accessToken = "YOUR_ACCESS_TOKEN";
                string defaultEmail = "user@example.com";

                // Create Gmail client instance.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Retrieve current configuration settings.
                    string currentToken = gmailClient.AccessToken;
                    int currentTimeout = gmailClient.Timeout;
                    IWebProxy currentProxy = gmailClient.Proxy;

                    Console.WriteLine("Current Access Token: " + currentToken);
                    Console.WriteLine("Current Timeout (ms): " + currentTimeout);
                    Console.WriteLine("Current Proxy: " + (currentProxy?.GetProxy(new Uri("https://gmail.com"))?.ToString() ?? "None"));

                    // Modify configuration settings.
                    gmailClient.AccessToken = "NEW_ACCESS_TOKEN";
                    gmailClient.Timeout = 200000; // 200 seconds
                    gmailClient.Proxy = new WebProxy("http://proxy.example.com:8080");

                    // Verify updated configuration.
                    Console.WriteLine("Updated Access Token: " + gmailClient.AccessToken);
                    Console.WriteLine("Updated Timeout (ms): " + gmailClient.Timeout);
                    Console.WriteLine("Updated Proxy: " + (gmailClient.Proxy?.GetProxy(new Uri("https://gmail.com"))?.ToString() ?? "None"));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}