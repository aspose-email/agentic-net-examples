using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/exchange/user@domain.com/";
            string username = "user";
            string password = "password";

            // Initialize the WebDAV client
            using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(mailboxUri, username, password))
            {
                // -----------------------------------------------------------------
                // CREATE: Create a test folder under the Inbox
                // -----------------------------------------------------------------
                try
                {
                    string testFolderName = "TestFolder";
                    string inboxUri = client.MailboxInfo.InboxUri;
                    client.CreateFolder(testFolderName, inboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error creating folder: " + ex.Message);
                }

                // -----------------------------------------------------------------
                // CREATE: Create a simple email message
                // -----------------------------------------------------------------
                Aspose.Email.MailMessage message = new Aspose.Email.MailMessage();
                message.From = "user@domain.com";
                message.To = "recipient@domain.com";
                message.Subject = "Test Message";
                message.Body = "This is a test message created via Aspose.Email WebDAV client.";

                // -----------------------------------------------------------------
                // CREATE: Append the message to the test folder
                // -----------------------------------------------------------------
                try
                {
                    client.AppendMessage("TestFolder", message);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error appending message: " + ex.Message);
                }

                // -----------------------------------------------------------------
                // READ: List messages in the test folder and fetch each one
                // -----------------------------------------------------------------
                try
                {
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages("TestFolder");
                    foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("URI: " + info.UniqueUri);

                        // Fetch full message content
                        Aspose.Email.MailMessage fetched = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine("Body: " + fetched.Body);

                        // DELETE: Remove the message after processing
                        client.DeleteMessage(info.UniqueUri);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error listing or processing messages: " + ex.Message);
                }

                // -----------------------------------------------------------------
                // CREATE: Create an archive folder
                // -----------------------------------------------------------------
                try
                {
                    string archiveFolderName = "ArchiveFolder";
                    string inboxUri = client.MailboxInfo.InboxUri;
                    client.CreateFolder(archiveFolderName, inboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error creating archive folder: " + ex.Message);
                }

                // -----------------------------------------------------------------
                // UPDATE: Move any remaining messages from TestFolder to ArchiveFolder
                // -----------------------------------------------------------------
                try
                {
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection remaining = client.ListMessages("TestFolder");
                    foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in remaining)
                    {
                        client.MoveMessage(info, "ArchiveFolder");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error moving messages: " + ex.Message);
                }

                // -----------------------------------------------------------------
                // DELETE: Clean up folders
                // -----------------------------------------------------------------
                try
                {
                    client.DeleteFolder("TestFolder");
                    client.DeleteFolder("ArchiveFolder");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error deleting folders: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}