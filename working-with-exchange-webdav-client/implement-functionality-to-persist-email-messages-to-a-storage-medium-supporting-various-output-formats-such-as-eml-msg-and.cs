using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a simple email message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com", "Sender");
                message.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                message.Subject = "Sample Email";
                message.Body = "This is a sample email message created with Aspose.Email.";

                // Save as EML
                string emlPath = Path.Combine(outputDir, "sample.eml");
                try
                {
                    message.Save(emlPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"EML saved to: {emlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save EML: {ex.Message}");
                }

                // Save as MSG
                string msgPath = Path.Combine(outputDir, "sample.msg");
                try
                {
                    message.Save(msgPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"MSG saved to: {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG: {ex.Message}");
                }

                // Save as MHTML
                string mhtmlPath = Path.Combine(outputDir, "sample.mhtml");
                try
                {
                    message.Save(mhtmlPath, SaveOptions.DefaultMhtml);
                    Console.WriteLine($"MHTML saved to: {mhtmlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MHTML: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
