using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "EmailWithEmbeddedImages.eml";
            string outputPath = "EmailWithEmbeddedImages.mht";

            // Ensure the input file exists; create a minimal placeholder if it does not.
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

                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = new MailAddress("sender@example.com");
                        placeholder.To.Add(new MailAddress("receiver@example.com"));
                        placeholder.Subject = "Placeholder Email";
                        placeholder.Body = "This is a placeholder email.";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder email: {ex.Message}");
                    return;
                }
            }

            // Load the email and save it as MHTML with embedded resources.
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    MhtSaveOptions options = new MhtSaveOptions
                    {
                        // Keep inline images as embedded resources.
                        ExtractHTMLBodyResourcesAsAttachments = false,
                        SaveAttachments = true,
                        SkipInlineImages = false
                    };

                    message.Save(outputPath, options);
                }

                Console.WriteLine("Email successfully saved as MHTML.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert email to MHTML: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
