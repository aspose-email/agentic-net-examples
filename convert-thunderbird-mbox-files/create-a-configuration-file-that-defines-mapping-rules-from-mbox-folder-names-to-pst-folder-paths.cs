using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the configuration, MBOX source and PST destination
            const string configPath = "folderMapping.json";
            const string mboxPath = "mailbox.mbox";
            const string pstPath = "output.pst";

            // Ensure configuration file exists; create a minimal placeholder if missing
            if (!File.Exists(configPath))
            {
                var placeholder = new Dictionary<string, string>
                {
                    { "Inbox", "Inbox" },
                    { "Sent", "Sent Items" }
                };
                File.WriteAllText(configPath, JsonSerializer.Serialize(placeholder));
                Console.WriteLine($"Created placeholder configuration at '{configPath}'.");
            }

            // Load folder mapping configuration
            Dictionary<string, string> folderMapping;
            try
            {
                string json = File.ReadAllText(configPath);
                folderMapping = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read configuration: {ex.Message}");
                return;
            }

            // Verify MBOX source file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file '{mboxPath}' not found.");
                return;
            }

            // Create or open the PST file
            PersonalStorage pst;
            try
            {
                if (File.Exists(pstPath))
                {
                    pst = PersonalStorage.FromFile(pstPath);
                }
                else
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create/open PST: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Ensure predefined folders exist according to the mapping
                foreach (var targetFolder in new HashSet<string>(folderMapping.Values))
                {
                    // Attempt to get the folder; if it doesn't exist, create it as a custom folder
                    try
                    {
                        pst.RootFolder.GetSubFolder(targetFolder);
                    }
                    catch
                    {
                        pst.RootFolder.AddSubFolder(targetFolder);
                    }
                }

                // Open the MBOX reader with required options
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    while (true)
                    {
                        // Read the next message; returns null when no more messages are available
                        MailMessage message = mboxReader.ReadNextMessage();
                        if (message == null)
                            break;

                        // Determine target PST folder based on a simple rule:
                        // If the subject contains a known source folder name, map it; otherwise use "Inbox"
                        string targetFolderName = "Inbox";
                        foreach (var kvp in folderMapping)
                        {
                            if (message.Subject != null && message.Subject.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                            {
                                targetFolderName = kvp.Value;
                                break;
                            }
                        }

                        // Retrieve or create the target folder in PST
                        FolderInfo targetFolder;
                        try
                        {
                            targetFolder = pst.RootFolder.GetSubFolder(targetFolderName);
                        }
                        catch
                        {
                            targetFolder = pst.RootFolder.AddSubFolder(targetFolderName);
                        }

                        // Add the message to the PST folder
                        try
                        {
                            targetFolder.AddMessage(MapiMessage.FromMailMessage(message));
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to add message '{message.Subject}': {ex.Message}");
                        }
                    }
                }
            }

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
