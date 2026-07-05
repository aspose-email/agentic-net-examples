using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

namespace MboxToPstConversion
{
    // Author: Aspose.Email example – converts an MBOX file to PST by iterating each message.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths – adjust as needed.
                string mboxFilePath = "input.mbox";
                string pstFilePath = "output.pst";

                // Guard file system access.
                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                    return;
                }

                // Delete existing PST to avoid IOException.
                if (File.Exists(pstFilePath))
                {
                    try
                    {
                        File.Delete(pstFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unable to delete existing PST file: {ex.Message}");
                        return;
                    }
                }

                // Create the PST storage.
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Create or get the target folder inside PST.
                    const string pstFolderName = "ImportedMbox";
                    FolderInfo pstFolder = pst.RootFolder.GetSubFolder(pstFolderName) ??
                                          pst.RootFolder.AddSubFolder(pstFolderName);

                    // Create the MBOX reader with required options.
                    using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                    {
                        // Iterate through each message in the MBOX file.
                        MailMessage message;
                        while ((message = mboxReader.ReadNextMessage()) != null)
                        {
                            // Add the message to the PST folder.
                            pstFolder.AddMessage(MapiMessage.FromMailMessage(message));
                        }
                    }
                }

                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
