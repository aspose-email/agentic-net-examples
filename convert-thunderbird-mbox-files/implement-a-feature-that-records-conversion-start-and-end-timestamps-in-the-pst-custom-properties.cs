using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure input MBOX exists; create minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string pstDir = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDir) && !Directory.Exists(pstDir))
            {
                try
                {
                    Directory.CreateDirectory(pstDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Record conversion start time
            DateTime conversionStart = DateTime.UtcNow;

            // Define a handler to add the start timestamp to each message before it is added to PST
            MailStorageConverter.MailHandler handler = (MailMessage mailMsg) =>
            {
                // Convert MailMessage to MapiMessage
                MapiMessage mapiMsg = MapiMessage.FromMailMessage(mailMsg);

                // Add custom property for conversion start
                string startStr = conversionStart.ToString("o");
                mapiMsg.AddCustomProperty(
                    MapiPropertyType.PT_UNICODE,
                    Encoding.Unicode.GetBytes(startStr),
                    "ConversionStart");
                // The modified MapiMessage will be used by the converter automatically
            };

            // Perform conversion
            try
            {
                MailStorageConverter.MboxToPst(mboxPath, pstPath, handler);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Record conversion end time
            DateTime conversionEnd = DateTime.UtcNow;

            // Open the resulting PST and add the end timestamp to each message
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all subfolders of the root folder
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                        {
                            // Extract the message, add the end timestamp, and update it in PST
                            using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                            {
                                string endStr = conversionEnd.ToString("o");
                                mapiMsg.AddCustomProperty(
                                    MapiPropertyType.PT_UNICODE,
                                    Encoding.Unicode.GetBytes(endStr),
                                    "ConversionEnd");

                                // Update the message with the new properties
                                pst.ChangeMessage(msgInfo.EntryIdString, mapiMsg.Properties);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add end timestamps: {ex.Message}");
                return;
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
