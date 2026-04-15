using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder values to avoid unwanted network calls.
            if (exchangeUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange contact retrieval.");
                return;
            }

            // Create and connect the Exchange client.
            using (ExchangeClient client = new ExchangeClient(exchangeUri, new System.Net.NetworkCredential(username, password)))
            {
                // Folder URI for the default contacts folder.
                string contactsFolderUri = client.GetFolderInfo("contacts").Uri;

                // Retrieve all contacts (may be large).
                MapiContact[] allContacts = client.ListContacts(contactsFolderUri);

                // Define page size for pagination.
                const int pageSize = 100;
                int totalContacts = allContacts.Length;
                int totalPages = (totalContacts + pageSize - 1) / pageSize;

                for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
                {
                    int start = pageIndex * pageSize;
                    int count = Math.Min(pageSize, totalContacts - start);
                    List<MapiContact> page = new List<MapiContact>(count);

                    for (int i = 0; i < count; i++)
                    {
                        page.Add(allContacts[start + i]);
                    }

                    // Process the current page of contacts.
                    Console.WriteLine($"Page {pageIndex + 1}/{totalPages}:");
                    foreach (MapiContact contact in page)
                    {
                        Console.WriteLine($"- {contact.NameInfo.DisplayName}");
                    }

                    // Optional: insert a short delay to simulate throttling or to avoid overwhelming the server.
                    // System.Threading.Thread.Sleep(100);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
