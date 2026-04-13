using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstManifest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";
                string outputPath = "manifest.json";

                // Verify PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                List<ManifestEntry> manifest = new List<ManifestEntry>();

                // Open PST and enumerate messages
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all folders recursively
                    Queue<FolderInfo> folders = new Queue<FolderInfo>();
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
                        foreach (MessageInfo msgInfo in currentFolder.EnumerateMessages())
                        {
                            using (MapiMessage message = pst.ExtractMessage(msgInfo))
                            {
                                // Calculate message size by saving to a memory stream
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    message.Save(ms);
                                    long sizeInBytes = ms.Length;

                                    ManifestEntry entry = new ManifestEntry
                                    {
                                        Subject = message.Subject ?? string.Empty,
                                        SizeInBytes = sizeInBytes,
                                        AttachmentCount = message.Attachments?.Count ?? 0,
                                        FolderPath = currentFolder.RetrieveFullPath()
                                    };
                                    manifest.Add(entry);
                                }
                            }
                        }
                    }
                }

                // Serialize manifest to JSON
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(manifest, jsonOptions);

                // Write JSON to file
                File.WriteAllText(outputPath, json);
                Console.WriteLine($"Manifest written to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Helper class for JSON manifest entries
        private class ManifestEntry
        {
            public string Subject { get; set; }
            public long SizeInBytes { get; set; }
            public int AttachmentCount { get; set; }
            public string FolderPath { get; set; }
        }
    }
}
