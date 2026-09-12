using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Ensure the output directory exists
            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create and configure a simple email message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("sender@example.com");
                mailMessage.To.Add(new MailAddress("recipient@example.com"));
                mailMessage.Subject = "Sample Email";
                mailMessage.Body = "This is a sample email body.";

                // Save as EML
                string emlPath = Path.Combine(outputDirectory, "sample.eml");
                mailMessage.Save(emlPath, SaveOptions.DefaultEml);

                // Save as MSG (Unicode)
                string msgPath = Path.Combine(outputDirectory, "sample.msg");
                mailMessage.Save(msgPath, SaveOptions.DefaultMsgUnicode);

                // Save as MHTML
                string mhtmlPath = Path.Combine(outputDirectory, "sample.mhtml");
                mailMessage.Save(mhtmlPath, SaveOptions.DefaultMhtml);
            }

            Console.WriteLine("Email message saved in EML, MSG, and MHTML formats.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
