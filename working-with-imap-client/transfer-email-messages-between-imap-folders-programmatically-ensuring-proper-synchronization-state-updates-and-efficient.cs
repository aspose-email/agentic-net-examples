using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapFolderTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder guard: skip real network operations when using example credentials.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            bool usePlaceholders = host.Contains("example.com") ||
                                   username.Contains("example.com") ||
                                   password == "password";

            if (usePlaceholders)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            try
            {
                using (ImapClient client = new ImapClient())
                {
                    client.Host = host;
                    client.Port = 993;
                    client.SecurityOptions = SecurityOptions.SSLImplicit;
                    client.Username = username;
                    client.Password = password;

                    // Define source and destination folders.
                    string sourceFolder = "INBOX";
                    string destinationFolder = "Archive";

                    // Retrieve message identifiers from the source folder.
                    ImapMessageInfoCollection messageInfos = client.ListMessages(sourceFolder);
                    List<string> messageUids = new List<string>();
                    foreach (ImapMessageInfo info in messageInfos)
                    {
                        // UniqueId is a string in recent Aspose.Email versions.
                        messageUids.Add(info.UniqueId);
                    }

                    // Convert the list of UIDs to a comma‑separated string as required by older API overloads.
                    string uidList = string.Join(",", messageUids);

                    // Perform copy.
                    client.CopyMessages(sourceFolder, destinationFolder, uidList);

                    // Perform move.
                    client.MoveMessages(sourceFolder, destinationFolder, uidList);

                    // Example of moving an entire folder (including subfolders) to a new parent folder.
                    string newParentFolder = "ArchivesRoot";
                    string folderToMove = destinationFolder;
                    client.MoveFolder(newParentFolder, folderToMove);

                    Console.WriteLine("Message transfer operations completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
