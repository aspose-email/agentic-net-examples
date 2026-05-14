using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace PSTManifestGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                var manifest = new List<ManifestEntry>();

                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ProcessFolder(pst.RootFolder, pst, manifest);
                }

                string json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions { WriteIndented = true });
                string outputPath = "manifest.json";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                try
                {
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Manifest written to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write manifest: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, List<ManifestEntry> manifest)
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    long sizeInBytes;
                    using (var ms = new MemoryStream())
                    {
                        message.Save(ms);
                        sizeInBytes = ms.Length;
                    }

                    int attachmentCount = message.Attachments.Count;

                    var entry = new ManifestEntry
                    {
                        Subject = messageInfo.Subject,
                        Size = sizeInBytes,
                        AttachmentCount = attachmentCount
                    };

                    manifest.Add(entry);
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst, manifest);
            }
        }
    }

    public class ManifestEntry
    {
        public string Subject { get; set; }
        public long Size { get; set; }
        public int AttachmentCount { get; set; }
    }
}
