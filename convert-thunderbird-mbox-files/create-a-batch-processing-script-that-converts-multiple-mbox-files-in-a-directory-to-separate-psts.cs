using System;
using System.IO;
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
            // Input directory containing MBOX files
            string inputDirectory = @"C:\MboxInput";   // TODO: replace with actual path
            // Output directory for generated PST files
            string outputDirectory = @"C:\PstOutput"; // TODO: replace with actual path

            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string[] mboxFiles = Directory.GetFiles(inputDirectory, "*.mbox");
            if (mboxFiles.Length == 0)
            {
                Console.WriteLine("No MBOX files found to process.");
                return;
            }

            foreach (string mboxPath in mboxFiles)
            {
                string pstFileName = Path.GetFileNameWithoutExtension(mboxPath) + ".pst";
                string pstPath = Path.Combine(outputDirectory, pstFileName);

                // Create a new PST file (Unicode format)
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a folder inside PST to store messages
                    FolderInfo inboxFolder = pst.RootFolder.AddSubFolder("Inbox");

                    // Open MBOX reader with load options
                    using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                    {
                        MailMessage message;
                        while ((message = reader.ReadNextMessage()) != null)
                        {
                            // Convert MailMessage to MapiMessage
                            MapiMessage mapiMessage = MapiMessage.FromMailMessage(message);
                            // Add the message to the PST folder
                            inboxFolder.AddMessage(mapiMessage);
                        }
                    }
                }

                Console.WriteLine($"Converted '{Path.GetFileName(mboxPath)}' to PST: '{pstFileName}'");
            }

            Console.WriteLine("Batch conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
