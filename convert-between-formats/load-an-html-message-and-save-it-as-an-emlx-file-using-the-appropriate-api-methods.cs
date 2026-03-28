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

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                using (MailMessage message = MailMessage.Load(sourcePath))
                {
                    // Save as EMLX using the default EML save options; the .emlx extension will produce the correct format.
                    message.Save(targetPath, SaveOptions.DefaultEml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
