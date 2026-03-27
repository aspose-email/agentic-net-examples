using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.html";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = new MailAddress("sender@example.com");
                    placeholder.To.Add(new MailAddress("receiver@example.com"));
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder email.";
                    placeholder.Save(inputPath);
                }
            }

            // Load the EML message and save it as HTML with attachments saved.
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                MhtSaveOptions options = new MhtSaveOptions
                {
                    SaveAttachments = true
                };
                message.Save(outputPath, options);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
