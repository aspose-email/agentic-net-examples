using System;
using System.IO;
using Aspose.Email;

using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths configuration
            string pstFilePath = "sample.pst";
            string outputDirectory = "ExportedEmails";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Get the Inbox folder (replace with another folder if needed)
                FolderInfo folder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                if (folder == null)
                {
                    Console.Error.WriteLine("Inbox folder not found in PST.");
                    return;
                }

                // Enumerate messages in the folder
                foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                {
                    // Extract the message as MapiMessage
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Convert to MailMessage for easier handling
                        using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                        {
                            // Build file name based on received date
                            string safeDate = mailMessage.Date.ToString("yyyyMMdd_HHmmss");
                            string emlFilePath = Path.Combine(outputDirectory, $"{safeDate}.eml");

                            // Save as EML
                            try
                            {
                                mailMessage.Save(emlFilePath, SaveOptions.DefaultEml);
                                Console.WriteLine($"Saved: {emlFilePath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message '{safeDate}': {saveEx.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
