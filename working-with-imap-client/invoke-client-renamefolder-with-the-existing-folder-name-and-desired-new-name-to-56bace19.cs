using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize IMAP client with host, credentials and security options
                using (ImapClient client = new ImapClient("imap.example.com", "username", "password", SecurityOptions.Auto))
                {
                    // Rename the folder from "OldFolder" to "NewFolder"
                    client.RenameFolder("OldFolder", "NewFolder");
                    Console.WriteLine("Folder renamed successfully.");
                }
            }
            catch (Exception ex)
            {
                // Output any errors to the error stream
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}