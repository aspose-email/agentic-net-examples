using Aspose.Email;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

namespace AsposeEmailAsyncContactSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Placeholder Exchange server URI and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials/hosts
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder Exchange server URI detected. Skipping network call.");
                    return;
                }

                // Create async EWS client inside a using block to ensure disposal
                using (IAsyncEwsClient client = EWSClient.GetEWSClient(mailboxUri, username, password) as IAsyncEwsClient)
                {
                    if (client == null)
                    {
                        Console.Error.WriteLine("Failed to create async EWS client.");
                        return;
                    }

                    // Resolve contacts asynchronously without blocking the UI thread
                    string unresolvedEntry = "John Doe";
                    try
                    {
                        MapiContactCollection contacts = await client.ResolveMapiContactsAsync(unresolvedEntry, CancellationToken.None);
                        Console.WriteLine($"Resolved {contacts.Count} contact(s) for \"{unresolvedEntry}\":");
                        foreach (MapiContact contact in contacts)
                        {
                            // Use NameInfo.DisplayName instead of non‑existent Name property
                            string displayName = contact.NameInfo?.DisplayName ?? "(no name)";
                            Console.WriteLine($"- {displayName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error resolving contacts: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
