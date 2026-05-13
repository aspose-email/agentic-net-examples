using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source and target PST files
            string sourcePstPath = "source.pst";
            string targetPstPath = "target.pst";

            // Ensure source PST exists; create a minimal placeholder if missing
            if (!File.Exists(sourcePstPath))
            {
                using (PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode)) { }
                Console.WriteLine($"Created placeholder source PST at '{sourcePstPath}'.");
            }

            // Ensure target PST exists; create a minimal placeholder if missing
            if (!File.Exists(targetPstPath))
            {
                using (PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode)) { }
                Console.WriteLine($"Created placeholder target PST at '{targetPstPath}'.");
            }

            // Open both PST files
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
            {
                // Get the Contacts folder from each PST
                FolderInfo sourceContacts = sourcePst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                FolderInfo targetContacts = targetPst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                // Build a lookup of existing contacts in the target PST keyed by Subject (contact name)
                Dictionary<string, MessageInfo> targetContactMap = new Dictionary<string, MessageInfo>(StringComparer.OrdinalIgnoreCase);
                foreach (MessageInfo targetInfo in targetContacts.EnumerateMessages())
                {
                    using (MapiMessage targetMsg = targetPst.ExtractMessage(targetInfo))
                    {
                        string subject = targetMsg.Subject ?? string.Empty;
                        if (!string.IsNullOrEmpty(subject))
                        {
                            targetContactMap[subject] = targetInfo;
                        }
                    }
                }

                // Iterate through contacts in the source PST
                foreach (MessageInfo sourceInfo in sourceContacts.EnumerateMessages())
                {
                    using (MapiMessage sourceMsg = sourcePst.ExtractMessage(sourceInfo))
                    {
                        string subject = sourceMsg.Subject ?? string.Empty;
                        if (string.IsNullOrEmpty(subject))
                        {
                            continue; // Skip contacts without a subject/name
                        }

                        if (targetContactMap.TryGetValue(subject, out MessageInfo existingTargetInfo))
                        {
                            // Contact exists in target PST – compare modification times
                            using (MapiMessage targetMsg = targetPst.ExtractMessage(existingTargetInfo))
                            {
                                DateTime sourceTime = sourceMsg.ClientSubmitTime;
                                DateTime targetTime = targetMsg.ClientSubmitTime;

                                if (sourceTime > targetTime)
                                {
                                    // Source contact is newer – update the target contact
                                    targetContacts.UpdateMessage(existingTargetInfo.EntryIdString, sourceMsg);
                                    Console.WriteLine($"Updated contact '{subject}' in target PST.");
                                }
                                else
                                {
                                    // Target contact is newer or same – no action needed
                                    Console.WriteLine($"Skipped contact '{subject}' (target is newer or equal).");
                                }
                            }
                        }
                        else
                        {
                            // Contact does not exist in target PST – add it
                            string addedEntryId = targetContacts.AddMessage(sourceMsg);
                            Console.WriteLine($"Added new contact '{subject}' to target PST (EntryId: {addedEntryId}).");
                        }
                    }
                }
            }

            Console.WriteLine("Contact synchronization completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
