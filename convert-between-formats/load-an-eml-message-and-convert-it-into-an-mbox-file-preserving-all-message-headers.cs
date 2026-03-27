using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "input.eml";
            string mboxPath = "output.mbox";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                using (MailMessage placeholder = new MailMessage(
                    "placeholder@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder EML file."))
                {
                    placeholder.Save(emlPath, SaveOptions.DefaultEml);
                }
            }

            // Load the EML message.
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Prepare MBOX writer with default options.
                MboxSaveOptions saveOptions = new MboxSaveOptions();

                // Write the message to the MBOX file.
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, saveOptions))
                {
                    writer.WriteMessage(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
