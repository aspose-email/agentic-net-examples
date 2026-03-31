using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server configuration (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected – skipping execution.");
                return;
            }

            // Root folder where messages will be exported
            string outputRoot = "ExportedMail";

            // Ensure the root output directory exists
            try
            {
                if (!Directory.Exists(outputRoot))
                {
                    Directory.CreateDirectory(outputRoot);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            // Connect to the IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                try
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error configuring client security: {ex.Message}");
                    return;
                }

                // Start exporting from the default INBOX folder
                ExportFolder(client, "INBOX", outputRoot);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void ExportFolder(ImapClient client, string folderName, string localPath)
    {
        // Select the target folder on the server
        try
        {
            client.SelectFolder(folderName);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Cannot select folder '{folderName}': {ex.Message}");
            return;
        }

        // Create a corresponding local directory
        string folderPath = Path.Combine(localPath, SanitizePath(folderName));
        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating local folder '{folderPath}': {ex.Message}");
            return;
        }

        // Export all messages in the current folder
        try
        {
            foreach (ImapMessageInfo messageInfo in client.ListMessages())
            {
                string safeSubject = SanitizePath(messageInfo.Subject ?? "NoSubject");
                string fileName = $"{safeSubject}_{messageInfo.UniqueId}.msg";
                string filePath = Path.Combine(folderPath, fileName);

                // Save the message as an MSG file
                client.SaveMessage(messageInfo.UniqueId, filePath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error exporting messages from folder '{folderName}': {ex.Message}");
        }

        // Recursively process subfolders
        try
        {
            foreach (ImapFolderInfo subFolder in client.ListFolders(folderName))
            {
                ExportFolder(client, subFolder.Name, folderPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating subfolders of '{folderName}': {ex.Message}");
        }
    }

    static string SanitizePath(string name)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar, '_');
        }
        return name;
    }
}
