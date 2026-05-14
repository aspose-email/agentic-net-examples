using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string targetPath = "merged.pst";
            string sourcePath1 = "contacts1.pst";
            string sourcePath2 = "contacts2.pst";

            // Verify source files exist; if missing, just skip them.
            var sourceFiles = new List<string> { sourcePath1, sourcePath2 };
            foreach (var src in sourceFiles)
            {
                if (!File.Exists(src))
                {
                    Console.Error.WriteLine($"Source PST not found: {src}. It will be skipped.");
                }
            }

            // Create the target PST if it does not exist.
            if (!File.Exists(targetPath))
            {
                try
                {
                    PersonalStorage.Create(targetPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create target PST: {ex.Message}");
                    return;
                }
            }

            // Open the target PST and perform merging.
            using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPath))
            {
                // Get or create the Contacts folder in the target PST.
                FolderInfo targetContactsFolder = targetPst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (targetContactsFolder == null)
                {
                    targetContactsFolder = targetPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                }

                // Build a set of existing contact names to avoid duplicates.
                var existingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (MessageInfo msgInfo in targetContactsFolder.EnumerateMessages())
                {
                    using (MapiMessage msg = targetPst.ExtractMessage(msgInfo))
                    {
                        string name = msg.Subject ?? string.Empty;
                        existingNames.Add(name);
                    }
                }

                // Local function to import contacts from a source PST.
                void ImportFromSource(string sourcePath)
                {
                    if (!File.Exists(sourcePath))
                        return;

                    using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePath))
                    {
                        FolderInfo sourceContactsFolder = sourcePst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                        if (sourceContactsFolder == null)
                            return;

                        foreach (MessageInfo srcMsgInfo in sourceContactsFolder.EnumerateMessages())
                        {
                            using (MapiMessage srcMsg = sourcePst.ExtractMessage(srcMsgInfo))
                            {
                                string name = srcMsg.Subject ?? string.Empty;
                                if (!existingNames.Contains(name))
                                {
                                    targetContactsFolder.AddMessage(srcMsg);
                                    existingNames.Add(name);
                                }
                            }
                        }
                    }
                }

                // Import contacts from each source PST.
                ImportFromSource(sourcePath1);
                ImportFromSource(sourcePath2);
            }

            Console.WriteLine("Merging completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
