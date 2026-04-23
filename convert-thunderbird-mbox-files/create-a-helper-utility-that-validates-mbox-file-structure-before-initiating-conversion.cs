using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";
            string pstPath = "output.pst";

            // Verify MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Validate MBOX structure by reading all messages
            int messageCount = 0;
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                Aspose.Email.MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    // Each message is read successfully; count it
                    messageCount++;

                    // Dispose the message after processing
                    message.Dispose();
                }
            }

            Console.WriteLine($"MBOX validation succeeded. Total messages: {messageCount}");

            // Proceed with conversion only if there is at least one message
            if (messageCount == 0)
            {
                Console.Error.WriteLine("MBOX file contains no messages. Conversion aborted.");
                return;
            }

            // Convert MBOX to PST
            try
            {
                // Ensure the target PST file does not already exist or can be overwritten
                if (File.Exists(pstPath))
                {
                    File.Delete(pstPath);
                }

                // Perform conversion
                PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
                pst.Dispose();

                Console.WriteLine($"Conversion completed. PST saved to: {pstPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
