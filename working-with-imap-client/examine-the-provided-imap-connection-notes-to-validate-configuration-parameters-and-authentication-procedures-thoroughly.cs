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
            // IMAP connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls in CI
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping connection.");
                return;
            }

            // Create the IMAP client and validate the connection
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        client.ValidateCredentials();
                        Console.WriteLine("IMAP credentials are valid.");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP validation failed: {imapEx.Message}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unexpected error during validation: {ex.Message}");
                        return;
                    }

                    // Example additional check: list available folders
                    try
                    {
                        var folders = client.ListFolders();
                        Console.WriteLine("Available folders:");
                        foreach (var folder in folders)
                        {
                            Console.WriteLine($"- {folder.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list folders: {ex.Message}");
                    }
                }
            }
            catch (ImapException imapEx)
            {
                Console.Error.WriteLine($"IMAP client error: {imapEx.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating IMAP client: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
