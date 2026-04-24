using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls
            if (mailboxUri.Contains("example") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                        Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                        Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                        Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                        Console.WriteLine("Deleted Items URI: " + mailboxInfo.DeletedItemsUri);
                        Console.WriteLine("Outbox URI: " + mailboxInfo.OutboxUri);
                        Console.WriteLine("Calendar URI: " + mailboxInfo.CalendarUri);
                        Console.WriteLine("Contacts URI: " + mailboxInfo.ContactsUri);
                        Console.WriteLine("Tasks URI: " + mailboxInfo.TasksUri);
                        Console.WriteLine("Notes URI: " + mailboxInfo.NotesUri);
                        Console.WriteLine("Journal URI: " + mailboxInfo.JournalUri);
                        Console.WriteLine("Root URI: " + mailboxInfo.RootUri);
                        Console.WriteLine("Submission URI: " + mailboxInfo.SubmissionUri);
                        Console.WriteLine("Junke Mails URI: " + mailboxInfo.JunkeMailsUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error retrieving mailbox information: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create or connect Exchange client: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
