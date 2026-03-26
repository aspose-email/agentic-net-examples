using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "message.html";
            string targetPath = "message.emlx";

            // Ensure source HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(sourcePath))
            {
                try
                {
                    File.WriteAllText(sourcePath, "<html><body><p>Placeholder email content.</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML message and save it as EMLX.
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(sourcePath))
                {
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);
                    mailMessage.Save(targetPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}