using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "input.eml";
            string mboxPath = "output.mbox";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Load the EML message preserving all headers.
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading EML file: {loadEx.Message}");
                return;
            }

            // Write the message to an MBOX file.
            try
            {
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, new MboxSaveOptions()))
                {
                    writer.WriteMessage(message);
                }
            }
            catch (Exception writeEx)
            {
                Console.Error.WriteLine($"Error writing MBOX file: {writeEx.Message}");
                return;
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}