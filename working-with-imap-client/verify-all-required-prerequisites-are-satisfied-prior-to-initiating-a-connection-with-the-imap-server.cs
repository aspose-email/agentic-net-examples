using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapPrerequisiteCheck
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Verify that required parameters are provided
                if (string.IsNullOrWhiteSpace(host))
                {
                    Console.Error.WriteLine("IMAP host is not specified.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.Error.WriteLine("IMAP username is not specified.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("IMAP password is not specified.");
                    return;
                }

                // Create the IMAP client inside a using block to ensure disposal
                using (Aspose.Email.Clients.Imap.ImapClient imapClient = new Aspose.Email.Clients.Imap.ImapClient(host, port, username, password, Aspose.Email.Clients.SecurityOptions.SSLImplicit))
                {
                    // Optional: enable logging for troubleshooting
                    imapClient.EnableLogger = true;
                    imapClient.LogFileName = "imap_log.txt";

                    // Verify the connection by sending a NOOP command
                    try
                    {
                        imapClient.Noop();
                        Console.WriteLine("IMAP connection verified successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to verify IMAP connection: {ex.Message}");
                        return;
                    }

                    // Additional check: ensure the INBOX folder exists and can be selected
                    string inboxFolder = "INBOX";
                    try
                    {
                        imapClient.SelectFolder(inboxFolder);
                        Console.WriteLine($"Folder '{inboxFolder}' selected successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to select folder '{inboxFolder}': {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}