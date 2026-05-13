using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string inputPstPath = "input.pst";
            string outputOstPath = "output.ost";

            // Ensure input PST exists; create a minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created
                    }
                    Console.WriteLine($"Placeholder PST created at '{inputPstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load source PST
            PersonalStorage sourcePst;
            try
            {
                sourcePst = PersonalStorage.FromFile(inputPstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load source PST: {ex.Message}");
                return;
            }

            // Create destination PST (will be converted to OST later)
            PersonalStorage destPst;
            try
            {
                destPst = PersonalStorage.Create(outputOstPath, FileFormatVersion.Unicode);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create destination PST: {ex.Message}");
                return;
            }

            using (sourcePst)
            using (destPst)
            {
                // Copy messages from source root folder to destination root folder
                FolderInfo sourceRoot = sourcePst.RootFolder;
                FolderInfo destRoot = destPst.RootFolder;

                foreach (MessageInfo msgInfo in sourceRoot.EnumerateMessages())
                {
                    try
                    {
                        // Extract the full MAPI message
                        MapiMessage mapMsg = sourcePst.ExtractMessage(msgInfo);

                        // Preserve custom flags (e.g., Follow‑Up, Importance)
                        MapiMessageFlags flags = mapMsg.Flags;
                        mapMsg.SetMessageFlags(flags);

                        // Add the message to the destination folder
                        destRoot.AddMessage(mapMsg);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to copy message '{msgInfo.Subject}': {ex.Message}");
                        // Continue with next message
                    }
                }

                // Convert the destination PST to OST format and save
                try
                {
                    destPst.ConvertTo(FileFormat.Ost);
                    destPst.SaveAs(outputOstPath, FileFormat.Ost);
                    Console.WriteLine($"Conversion completed. OST saved at '{outputOstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert/save OST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
