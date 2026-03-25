using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Connection parameters (replace with actual values)
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                try
                {
                    // Create and use the EWS client
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Retrieve mailbox information
                        Aspose.Email.Clients.Exchange.ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                        Console.WriteLine("Mailbox URIs:");
                        Console.WriteLine("Inbox: " + mailboxInfo.InboxUri);
                        Console.WriteLine("Sent Items: " + mailboxInfo.SentItemsUri);
                        Console.WriteLine("Drafts: " + mailboxInfo.DraftsUri);
                        Console.WriteLine("Deleted Items: " + mailboxInfo.DeletedItemsUri);
                        Console.WriteLine("Calendar: " + mailboxInfo.CalendarUri);
                        Console.WriteLine("Contacts: " + mailboxInfo.ContactsUri);
                        Console.WriteLine("Tasks: " + mailboxInfo.TasksUri);
                        Console.WriteLine("Root: " + mailboxInfo.RootUri);

                        // Retrieve server version information
                        string versionInfo = client.GetVersionInfo();
                        Console.WriteLine("Server version: " + versionInfo);

                        // Retrieve mailbox size
                        long mailboxSize = client.GetMailboxSize();
                        Console.WriteLine("Mailbox size (bytes): " + mailboxSize);

                        // List mailboxes in the organization
                        Contact[] mailboxes = client.GetMailboxes();
                        Console.WriteLine("Mailboxes in organization:");
                        foreach (Contact mb in mailboxes)
                        {
                            if (mb.EmailAddresses.Count > 0)
                            {
                                Console.WriteLine("- " + mb.EmailAddresses[0].Address);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error connecting to Exchange server: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}