using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "TargetFolder";

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Delete the specified folder permanently
                client.DeleteFolder(folderName);
                Console.WriteLine($"Folder '{folderName}' has been deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
