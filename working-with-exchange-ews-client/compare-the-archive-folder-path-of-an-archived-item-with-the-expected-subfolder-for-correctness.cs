using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client (replace with real server URL and credentials)
            string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                // Expected archive folder URI (replace with the actual expected value)
                string expectedArchiveFolderUri = "https://mail.example.com/EWS/Exchange.asmx/Archive";


                // Skip external calls when placeholder credentials are used
                if (ewsUrl.Contains("example.com") || expectedArchiveFolderUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Retrieve information about the archive folder.
                // The folder name "Archive" corresponds to the one‑click archive feature.
                ExchangeFolderInfo archiveFolderInfo = client.GetFolderInfo("Archive");

                // Compare the retrieved folder URI with the expected URI.
                if (archiveFolderInfo.Uri != null && archiveFolderInfo.Uri.Equals(expectedArchiveFolderUri, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Archive folder path is correct.");
                }
                else
                {
                    Console.WriteLine("Archive folder path mismatch.");
                    Console.WriteLine($"Actual: {archiveFolderInfo.Uri}");
                    Console.WriteLine($"Expected: {expectedArchiveFolderUri}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
