using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Storage.Pst;

public class Program
{
    public static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Path to the backup PST file
            string pstFilePath = "backup.pst";

            // Verify that the PST file exists before attempting to open it
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstFilePath}");
                return;
            }

            // EWS connection parameters (replace with actual values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method (returns IEWSClient)
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Open the PST file
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                    {
                        // Restore the mailbox from the PST, preserving folder hierarchy and metadata
                        RestoreSettings restoreSettings = new RestoreSettings();
                        ewsClient.Restore(pst, restoreSettings);
                        Console.WriteLine("Mailbox restoration completed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Restoration error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}