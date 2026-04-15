using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = ""; // leave empty if not needed
            string sharedMailbox = "shared@example.com";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || sharedMailbox.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password, domain));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Retrieve delegate users for the shared mailbox
            ExchangeDelegateUserCollection delegateUsers = null;
            try
            {
                delegateUsers = client.ListDelegates(sharedMailbox);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list delegates: {ex.Message}");
                return;
            }

            // Prepare CSV data
            List<string> csvLines = new List<string>();
            csvLines.Add("DelegateEmail,Inbox,Calendar,Contacts,Journal,Notes,Tasks");

            foreach (ExchangeDelegateUser delegateUser in delegateUsers)
            {
                string email = delegateUser.UserInfo?.PrimarySmtpAddress ?? string.Empty;
                ExchangeDelegatePermissions perms = delegateUser.FolderPermissions;

                string inbox = perms.InboxFolderPermissionLevel.ToString();
                string calendar = perms.CalendarFolderPermissionLevel.ToString();
                string contacts = perms.ContactsFolderPermissionLevel.ToString();
                string journal = perms.JournalFolderPermissionLevel.ToString();
                string notes = perms.NotesFolderPermissionLevel.ToString();
                string tasks = perms.TasksFolderPermissionLevel.ToString();

                csvLines.Add($"{email},{inbox},{calendar},{contacts},{journal},{notes},{tasks}");
            }

            // Define output file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "DelegatePermissions.csv");
            string outputDir = Path.GetDirectoryName(outputPath);

            // Ensure directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Write CSV to file
            try
            {
                File.WriteAllLines(outputPath, csvLines);
                Console.WriteLine($"Delegate permissions exported to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose client
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
