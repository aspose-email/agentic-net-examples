using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder values are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder mailbox URI detected. Skipping network call.");
                return;
            }

            // Inspect server response headers to verify WebDAV support
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mailboxUri);
            request.Method = "OPTIONS";
            request.Credentials = new NetworkCredential(username, password);
            request.Timeout = 10000;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    string davHeader = response.Headers["DAV"];
                    if (string.IsNullOrEmpty(davHeader) || !davHeader.Contains("1"))
                    {
                        Console.Error.WriteLine("WebDAV is not supported by the Exchange server.");
                        return;
                    }
                }
            }
            catch (WebException ex)
            {
                Console.Error.WriteLine($"Failed to retrieve server headers: {ex.Message}");
                return;
            }

            // Initialize ExchangeClient after confirming WebDAV support
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine($"Exchange server version: {versionInfo}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error using ExchangeClient: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
