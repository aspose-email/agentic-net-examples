using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid external calls during CI.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Skipping Gmail folder rename because placeholder credentials are used.");
                return;
            }

            // Create and connect the IMAP client.
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Rename the folder "Temp" to "Processed".
                    client.RenameFolder("Temp", "Processed");
                    Console.WriteLine("Folder renamed successfully.");

                    // Verify that the new folder exists.
                    bool exists = client.ExistFolder("Processed");
                    Console.WriteLine($"Verification: Processed folder exists = {exists}");
                }
                catch (Exception ex)
                {
                    // Handle errors related to folder operations.
                    Console.Error.WriteLine($"Error during folder rename: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
