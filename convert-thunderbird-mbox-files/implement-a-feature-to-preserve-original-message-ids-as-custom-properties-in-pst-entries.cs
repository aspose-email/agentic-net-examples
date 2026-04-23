using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open PST and process messages
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    FolderInfo rootFolder = pst.RootFolder;

                    foreach (MessageInfo msgInfo in rootFolder.EnumerateMessages())
                    {
                        try
                        {
                            using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                            {
                                string originalId = mapiMsg.InternetMessageId;
                                if (!string.IsNullOrEmpty(originalId))
                                {
                                    // Encode the original ID as Unicode bytes
                                    byte[] idBytes = Encoding.Unicode.GetBytes(originalId);

                                    // Add custom property named "OriginalMessageId"
                                    mapiMsg.AddCustomProperty(MapiPropertyType.PT_UNICODE, idBytes, "OriginalMessageId");

                                    // Update the message in the PST with the modified properties
                                    pst.ChangeMessage(msgInfo.EntryIdString, mapiMsg.Properties);
                                }
                            }
                        }
                        catch (Exception exMsg)
                        {
                            Console.Error.WriteLine($"Error processing message '{msgInfo.Subject}': {exMsg.Message}");
                            // Continue with next message
                        }
                    }
                }
            }
            catch (Exception exPst)
            {
                Console.Error.WriteLine($"Failed to open or process PST file: {exPst.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
