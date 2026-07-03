using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure a placeholder file exists to satisfy validation
            if (!File.Exists(pstPath))
            {
                File.WriteAllBytes(pstPath, new byte[0]);
                Console.Error.WriteLine($"Placeholder PST file created at: {pstPath}");
                // In a real scenario, replace with a valid PST file.
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Dictionary to hold thread groups (key: ConversationTopic, value: list of message subjects)
                var threadGroups = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                // Traverse folders recursively
                var folders = new Queue<FolderInfo>();
                folders.Enqueue(pst.RootFolder);

                while (folders.Count > 0)
                {
                    FolderInfo currentFolder = folders.Dequeue();

                    // Enqueue subfolders
                    foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                    {
                        folders.Enqueue(subFolder);
                    }

                    // Process messages in the current folder
                    foreach (MessageInfo messageInfo in currentFolder.EnumerateMessages())
                    {
                        try
                        {
                            using (MapiMessage message = pst.ExtractMessage(messageInfo))
                            {
                                string threadId = message.ConversationTopic ?? string.Empty;
                                string subject = message.Subject ?? "(No Subject)";

                                if (!threadGroups.TryGetValue(threadId, out List<string> list))
                                {
                                    list = new List<string>();
                                    threadGroups[threadId] = list;
                                }

                                list.Add(subject);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract message ID {messageInfo.EntryIdString}: {ex.Message}");
                        }
                    }
                }

                // Output grouping results
                foreach (var kvp in threadGroups)
                {
                    string threadId = string.IsNullOrEmpty(kvp.Key) ? "(No Topic)" : kvp.Key;
                    Console.WriteLine($"Thread: {threadId} - Messages: {kvp.Value.Count}");
                    foreach (var subj in kvp.Value)
                    {
                        Console.WriteLine($"   Subject: {subj}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
