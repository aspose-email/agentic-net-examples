using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "ampMessage.eml";
            string outputPath = "ampMessage_updated.eml";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (var placeholder = new AmpMessage())
                {
                    placeholder.Subject = "Placeholder Subject";
                    placeholder.Body = "Placeholder body";
                    placeholder.Save(inputPath);
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the AMP message, update the subject, and save
            using (AmpMessage amp = (AmpMessage)MailMessage.Load(inputPath))
            {
                amp.Subject = "New Campaign Subject";
                amp.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
