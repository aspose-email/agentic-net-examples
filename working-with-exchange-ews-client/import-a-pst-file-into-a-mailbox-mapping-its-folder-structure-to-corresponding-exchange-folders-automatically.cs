using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and credentials
            string pstFilePath = "source.pst";
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Initialize Exchange client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                    {
                        // Restore settings (default)
                        RestoreSettings restoreSettings = new RestoreSettings();

                        // Import PST into mailbox, preserving folder hierarchy
                        client.Restore(pst, restoreSettings);
                        Console.WriteLine("PST import completed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
