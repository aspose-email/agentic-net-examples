using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mapi;

namespace ConvertMboxToPst
{
    class Program
    {
        static void Main()
        {
            // Author note: Simple console app that converts a Thunderbird MBOX file to a PST file
            // and adds each converted message to the root folder of the PST.

            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard file I/O
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            try
            {
                // Create a new PST file (Unicode format)
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Open the MBOX storage for reading
                    using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                    {
                        // Iterate through each message in the MBOX file
                        foreach (MboxMessageInfo mboxInfo in mboxReader.EnumerateMessageInfo())
                        {
                            // Extract the full MIME message as a MailMessage object
                            MailMessage mailMessage = mboxReader.ExtractMessage(mboxInfo.EntryId, new EmlLoadOptions());

                            // Convert MailMessage to MapiMessage (required for PST storage)
                            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                            // Add the converted message to the root folder of the PST
                            pst.RootFolder.AddMessage(mapiMessage);
                        }
                    }

                    // PST is saved automatically when disposed
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
