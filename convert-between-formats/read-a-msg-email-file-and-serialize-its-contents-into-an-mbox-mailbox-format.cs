using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

namespace ConvertMsgToMbox
{
    class Program
    {
        static void Main()
        {
            const string inputMsgPath = "input.msg";
            const string outputMboxPath = "output.mbox";

            // Validate input file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input MSG file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputMboxPath));
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            try
            {
                // Load MSG and convert to MailMessage
                MapiMessage mapiMsg = MapiMessage.Load(inputMsgPath);
                MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

                // Create a temporary PST to hold the message
                string tempPstPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pst");
                using (PersonalStorage pst = PersonalStorage.Create(tempPstPath, FileFormatVersion.Unicode))
                {
                    // Add the message to the root folder of the PST
                    pst.RootFolder.AddMessage(MapiMessage.FromMailMessage(mailMsg));

                    // Convert the PST to MBOX using the convenience method
                    MailboxConverter.ConvertPersonalStorageToMbox(pst, outputMboxPath, null);
                }

                // Delete the temporary PST file
                try { File.Delete(tempPstPath); } catch { /* ignore cleanup errors */ }

                // Read back the created MBOX to verify
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(outputMboxPath, new MboxLoadOptions()))
                {
                    MailMessage nextMsg;
                    while ((nextMsg = reader.ReadNextMessage()) != null)
                    {
                        Console.WriteLine($"Subject: {nextMsg.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}
