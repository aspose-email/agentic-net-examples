using Aspose.Email;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize ImapClient with a custom certificate validation callback that accepts all certificates
            using (ImapClient client = new ImapClient(host, port, username, password,
                (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true))
            {
                try
                {
                    // Trigger TLS handshake by listing folders
                    var folders = client.ListFolders();
                    foreach (var folder in folders)
                    {
                        Console.WriteLine(folder.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
