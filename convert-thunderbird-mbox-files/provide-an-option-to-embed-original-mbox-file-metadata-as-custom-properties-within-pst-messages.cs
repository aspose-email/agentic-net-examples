using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output PST file path
            string pstPath = "output.pst";
            // Folder name inside PST where messages will be stored
            string pstFolderName = "Imported";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // Write a minimal empty MBOX content
                        byte[] empty = Encoding.UTF8.GetBytes(string.Empty);
                        placeholder.Write(empty, 0, empty.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            try
            {
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST directory: {ex.Message}");
                return;
            }

            // Create a new PST file (Unicode format)
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Create or get the target folder inside PST
                FolderInfo targetFolder = pst.RootFolder.AddSubFolder(pstFolderName);

                // Open the MBOX storage for reading
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    while (true)
                    {
                        // Read the next message; null indicates end of file
                        MailMessage mailMessage = mboxReader.ReadNextMessage();
                        if (mailMessage == null)
                            break;

                        // Convert MailMessage to MapiMessage
                        using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                        {
                            // Retrieve original Message-ID header (if present) to embed as a custom property
                            string originalMessageId = mailMessage.MessageId ?? string.Empty;

                            // Encode the property value as Unicode bytes
                            byte[] propertyBytes = Encoding.Unicode.GetBytes(originalMessageId);

                            // Add custom property named "OriginalMessageId"
                            mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, propertyBytes, "OriginalMessageId");

                            // Add the message to the PST folder
                            targetFolder.AddMessage(mapiMessage);
                        }

                        // Dispose the MailMessage after processing
                        mailMessage.Dispose();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
