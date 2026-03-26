using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "input.msg";
            string pstPath = "temp.pst";
            string mboxPath = "output.mbox";

            // Verify input MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load MSG into MapiMessage
            MapiMessage mapiMsg;
            try
            {
                mapiMsg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Convert MapiMessage to MailMessage (required for PST storage)
            MailMessage mailMsg;
            try
            {
                mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion to MailMessage failed: {ex.Message}");
                return;
            }

            // Create a temporary PST file and add the message
            try
            {
                // Ensure any existing temp PST is removed
                if (File.Exists(pstPath))
                {
                    File.Delete(pstPath);
                }

                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Add message to the root folder
                    pst.RootFolder.AddMessage(mapiMsg);
                    // Convert the PST to MBOX
                    try
                    {
                        MailboxConverter.ConvertPersonalStorageToMbox(pst, mboxPath, null);
                        Console.WriteLine($"MBOX file created at: {mboxPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert PST to MBOX: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"PST creation or processing failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}