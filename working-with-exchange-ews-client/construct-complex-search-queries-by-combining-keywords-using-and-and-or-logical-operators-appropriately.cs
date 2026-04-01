using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsSearchExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Placeholder connection details – replace with real values when running against a live server
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip actual network call when placeholder data is detected (prevents CI failures)
                if (mailboxUri.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                    return;
                }

                // Create the EWS client using the factory method (no direct constructor)
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // ------------------------------
                    // Example 1: AND query
                    // Subject contains "Report" AND From contains "alice@example.com"
                    // ------------------------------
                    ExchangeQueryBuilder andBuilder = new ExchangeQueryBuilder();
                    andBuilder.Subject.Contains("Report");
                    andBuilder.From.Contains("alice@example.com");
                    MailQuery andQuery = andBuilder.GetQuery();

                    // List messages that match the AND query
                    ExchangeMessageInfoCollection andMessages = client.ListMessages(client.MailboxInfo.InboxUri, andQuery);
                    Console.WriteLine($"AND query returned {andMessages.Count} message(s).");

                    // ------------------------------
                    // Example 2: OR query
                    // From contains "alice@example.com" OR From contains "bob@example.com"
                    // ------------------------------
                    ExchangeQueryBuilder orBuilder = new ExchangeQueryBuilder();
                    MailQuery fromAlice = orBuilder.From.Contains("alice@example.com");
                    MailQuery fromBob = orBuilder.From.Contains("bob@example.com");
                    MailQuery orQuery = orBuilder.Or(fromAlice, fromBob);

                    // List messages that match the OR query
                    ExchangeMessageInfoCollection orMessages = client.ListMessages(client.MailboxInfo.InboxUri, orQuery);
                    Console.WriteLine($"OR query returned {orMessages.Count} message(s).");
                }
            }
            catch (Exception ex)
            {
                // Friendly error output – prevents unhandled exceptions from crashing the program
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
