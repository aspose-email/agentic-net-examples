using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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

            // Ensure the PST file exists; create an empty one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Dictionary to map hash -> list of locations.
                    Dictionary<string, List<string>> messageMap = new Dictionary<string, List<string>>();

                    // Start processing from the root folder.
                    ProcessFolder(pst.RootFolder, pst.RootFolder.DisplayName, pst, messageMap);

                    // Output duplicates.
                    foreach (KeyValuePair<string, List<string>> entry in messageMap)
                    {
                        if (entry.Value.Count > 1)
                        {
                            Console.WriteLine($"Duplicate messages (Hash: {entry.Key}) found in the following locations:");
                            foreach (string location in entry.Value)
                            {
                                Console.WriteLine($"  {location}");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively processes a folder and its subfolders.
    private static void ProcessFolder(FolderInfo folder, string folderPath, PersonalStorage pst, Dictionary<string, List<string>> map)
    {
        // Process messages in the current folder.
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    string subject = message.Subject ?? string.Empty;
                    string body = message.Body ?? string.Empty;

                    // Compute SHA256 hash of subject + body.
                    string hash = ComputeHash(subject + body);

                    string location = $"{folderPath}\\{subject}";

                    if (!map.ContainsKey(hash))
                    {
                        map[hash] = new List<string>();
                    }
                    map[hash].Add(location);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message ID {messageInfo.EntryIdString}: {ex.Message}");
                // Continue with next message.
            }
        }

        // Recurse into subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            string subFolderPath = $"{folderPath}\\{subFolder.DisplayName}";
            ProcessFolder(subFolder, subFolderPath, pst, map);
        }
    }

    // Computes a hex string representation of SHA256 hash for the given input.
    private static string ComputeHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
