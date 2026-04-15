using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace EnumeratePstRecipients
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input PST file path
                string pstPath = "sample.pst";
                // Output text file path
                string outputPath = "recipients.txt";

                // Verify PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Open PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Access a specific folder (e.g., Inbox)
                    FolderInfo folder = pst.RootFolder.GetSubFolder("Inbox");

                    // Open writer for logging email addresses
                    using (StreamWriter writer = new StreamWriter(outputPath, false))
                    {
                        // Enumerate all messages in the folder
                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            // Extract recipients for the current message
                            MapiRecipientCollection recipients = pst.ExtractRecipients(messageInfo);

                            // Log each recipient's email address
                            foreach (MapiRecipient recipient in recipients)
                            {
                                string email = recipient.EmailAddress;
                                if (!string.IsNullOrEmpty(email))
                                {
                                    writer.WriteLine(email);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
