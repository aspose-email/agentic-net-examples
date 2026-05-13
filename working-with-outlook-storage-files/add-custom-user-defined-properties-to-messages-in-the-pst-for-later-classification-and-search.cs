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
            const string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process messages in the root folder.
                    FolderInfo rootFolder = pst.RootFolder;

                    foreach (MessageInfo messageInfo in rootFolder.EnumerateMessages())
                    {
                        try
                        {
                            // Extract the full MAPI message.
                            MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                            // Add a custom Unicode property.
                            string propertyName = "X-MyCategory";
                            string propertyValue = "CategoryA";
                            byte[] valueBytes = Encoding.Unicode.GetBytes(propertyValue);
                            mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, propertyName);

                            // Update the message back into the PST.
                            rootFolder.UpdateMessage(messageInfo.EntryIdString, mapiMessage);

                            Console.WriteLine($"Added custom property to message: {messageInfo.Subject}");
                        }
                        catch (Exception msgEx)
                        {
                            Console.Error.WriteLine($"Error processing message '{messageInfo.Subject}': {msgEx.Message}");
                            // Continue with next message.
                        }
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Failed to open or process PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
