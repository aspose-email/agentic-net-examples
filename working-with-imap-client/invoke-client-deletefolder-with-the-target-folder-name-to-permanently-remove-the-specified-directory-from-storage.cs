using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "TargetFolder";

            // Create and dispose the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Delete the specified folder permanently
                    imapClient.DeleteFolder(folderName);
                    Console.WriteLine($"Folder '{folderName}' deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}