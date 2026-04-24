using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder IMAP server credentials.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are used.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP restore operation.");
                return;
            }

            // Create a minimal in‑memory PST storage.
            using (MemoryStream pstStream = new MemoryStream())
            {
                // Create an empty PST file in the stream.
                using (PersonalStorage pst = PersonalStorage.Create(pstStream, FileFormatVersion.Unicode))
                {
                    // Ensure the stream position is at the beginning before restoring.
                    pstStream.Position = 0;

                    // Connect to the IMAP server.
                    try
                    {
                        using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
                        {
                            // Restore the PST contents directly from the memory stream.
                            client.Restore(pst, new RestoreSettings());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
