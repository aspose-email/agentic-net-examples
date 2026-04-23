using Aspose.Email.Storage.Pst;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values)
            string host = "imap.example.com";
            string username = "username";
            string password = "password";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create and use the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Validate credentials
                    bool isValid = client.ValidateCredentials();
                    if (!isValid)
                    {
                        Console.Error.WriteLine("Authentication failed. Please check your credentials.");
                        return;
                    }

                    // List all mail folders
                    ImapFolderInfoCollection folders = client.ListFolders();
                    Console.WriteLine("Available mail folders:");
                    foreach (ImapFolderInfo folder in folders)
                    {
                        Console.WriteLine("- " + folder.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during IMAP operations: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
