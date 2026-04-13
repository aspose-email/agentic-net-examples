using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths and credentials
            string pstPath = "input.pst";
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load PST and restore to Exchange mailbox
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Create Exchange client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    try
                    {
                        // Restore all folders and messages from PST to the mailbox
                        RestoreSettings settings = new RestoreSettings();
                        client.Restore(pst, settings);
                        Console.WriteLine("PST import completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during restore operation: {ex.Message}");
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
