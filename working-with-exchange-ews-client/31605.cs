using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailDeleteOldDeletedItems
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Mailbox connection settings – replace with real values or set via environment variables.
                string mailboxUri = Environment.GetEnvironmentVariable("MAILBOX_URI");
                string username = Environment.GetEnvironmentVariable("MAILBOX_USERNAME");
                string password = Environment.GetEnvironmentVariable("MAILBOX_PASSWORD");

                // Guard against placeholder or missing credentials.
                if (string.IsNullOrWhiteSpace(mailboxUri) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("Mailbox connection information is missing or contains placeholders. Skipping operation.");
                    return;
                }

                // Create EWS client inside a try/catch to handle connection/authentication errors.
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Obtain the Deleted Items folder information.
                        ExchangeFolderInfo deletedFolderInfo = client.GetFolderInfo("Deleted Items");
                        if (deletedFolderInfo == null || string.IsNullOrEmpty(deletedFolderInfo.Uri))
                        {
                            Console.Error.WriteLine("Unable to locate the Deleted Items folder.");
                            return;
                        }

                        // Build a query for items older than 90 days.
                        MailQueryBuilder queryBuilder = new MailQueryBuilder();
                        DateTime cutoffDate = DateTime.UtcNow.AddDays(-90);
                        queryBuilder.InternalDate.Before(cutoffDate);

                        // Retrieve messages that match the query.
                        ExchangeMessageInfoCollection messages = client.ListMessages(deletedFolderInfo.Uri, queryBuilder.GetQuery());
                        if (messages == null || messages.Count == 0)
                        {
                            Console.WriteLine("No items older than 90 days were found in Deleted Items.");
                            return;
                        }

                        // Delete each message.
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            try
                            {
                                // DeleteMessage expects the unique URI of the item.
                                client.DeleteItem(messageInfo.UniqueUri, new DeletionOptions(DeletionType.MoveToDeletedItems));
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to delete message '{messageInfo.UniqueUri}': {ex.Message}");
                            }
                        }

                        Console.WriteLine($"Deleted {messages.Count} item(s) older than 90 days from Deleted Items.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect to the mailbox: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
