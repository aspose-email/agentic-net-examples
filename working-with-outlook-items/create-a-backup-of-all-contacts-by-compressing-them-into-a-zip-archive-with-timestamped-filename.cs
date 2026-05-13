using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                mailboxUri.Contains("example.com") ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping backup operation.");
                return;
            }

            // Prepare output directory.
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "BackupOutput");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Timestamped filenames.
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string pstFilePath = Path.Combine(outputDirectory, $"Contacts_{timestamp}.pst");
            string zipFilePath = Path.Combine(outputDirectory, $"ContactsBackup_{timestamp}.zip");

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Identify the Contacts folder.
                    string contactsFolderName = "Contacts";
                    ExchangeFolderInfo contactsFolderInfo = client.GetFolderInfo(contactsFolderName);

                    // Prepare folder collection for backup.
                    ExchangeFolderInfoCollection foldersToBackup = new ExchangeFolderInfoCollection();
                    foldersToBackup.Add(contactsFolderInfo);

                    // Perform the backup to a PST file.
                    BackupOptions backupOptions = new BackupOptions();
                    client.Backup(foldersToBackup, pstFilePath, backupOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange backup: {ex.Message}");
                    return;
                }
            }

            // Compress the PST file into a ZIP archive.
            try
            {
                using (FileStream zipStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                    {
                        archive.CreateEntryFromFile(pstFilePath, Path.GetFileName(pstFilePath));
                    }
                }

                // Optionally delete the intermediate PST file.
                if (File.Exists(pstFilePath))
                {
                    File.Delete(pstFilePath);
                }

                Console.WriteLine($"Contacts successfully backed up to: {zipFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during ZIP creation: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
