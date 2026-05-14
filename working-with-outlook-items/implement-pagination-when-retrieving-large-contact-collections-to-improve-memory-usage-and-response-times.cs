using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

namespace AsposeEmailPaginationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create and use the Exchange client safely.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Folder URI where contacts are stored.
                    string contactsFolderUri = "/contacts";

                    // Retrieve all contacts from the folder.
                    MapiContact[] allContacts;
                    try
                    {
                        allContacts = client.ListContacts(contactsFolderUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list contacts: {ex.Message}");
                        return;
                    }

                    // Define page size for pagination.
                    const int pageSize = 100;
                    int total = allContacts.Length;
                    Console.WriteLine($"Total contacts retrieved: {total}");

                    // Process contacts in pages to reduce memory pressure.
                    for (int offset = 0; offset < total; offset += pageSize)
                    {
                        IEnumerable<MapiContact> page = allContacts.Skip(offset).Take(pageSize);
                        Console.WriteLine($"Processing contacts {offset + 1} to {Math.Min(offset + pageSize, total)}:");

                        foreach (MapiContact contact in page)
                        {
                            // DisplayName is available via the NameInfo property.
                            string displayName = contact.NameInfo?.DisplayName ?? "(no name)";
                            Console.WriteLine($"- {displayName}");
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
}
