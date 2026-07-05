using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input directory containing .msg files and output PST path
            string inputDir = "input";
            string outputPstPath = "filtered_output.pst";

            // Ensure input directory exists; if not, create it (empty placeholder)
            if (!Directory.Exists(inputDir))
            {
                Console.Error.WriteLine($"Input directory '{inputDir}' does not exist. Creating empty placeholder.");
                Directory.CreateDirectory(inputDir);
                // No .msg files to process; exit gracefully
                return;
            }

            // Gather all .msg files from the input directory
            string[] msgFiles = Directory.GetFiles(inputDir, "*.msg", SearchOption.TopDirectoryOnly);

            // Define keywords to filter messages (case‑insensitive)
            string[] keywords = new[] { "invoice", "report" };

            // Load messages and filter using LINQ logic
            List<MapiMessage> filteredMessages = new List<MapiMessage>();
            foreach (string path in msgFiles)
            {
                MapiMessage message = MapiMessage.Load(path);
                bool matches = keywords.Any(k =>
                    (message.Subject ?? string.Empty).IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (message.Body ?? string.Empty).IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
                if (matches)
                {
                    filteredMessages.Add(message);
                }
            }

            // If no messages match, inform the user and exit
            if (!filteredMessages.Any())
            {
                Console.WriteLine("No messages matched the specified keywords.");
                return;
            }

            // Create (or overwrite) the PST file
            using (PersonalStorage pst = PersonalStorage.Create(outputPstPath, FileFormatVersion.Unicode))
            {
                // Create a subfolder to hold filtered messages
                FolderInfo filteredFolder = pst.RootFolder.AddSubFolder("Filtered");

                // Add each filtered message to the PST folder
                foreach (MapiMessage msg in filteredMessages)
                {
                    filteredFolder.AddMessage(msg);
                }

                Console.WriteLine($"Added {filteredMessages.Count} filtered messages to '{outputPstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
