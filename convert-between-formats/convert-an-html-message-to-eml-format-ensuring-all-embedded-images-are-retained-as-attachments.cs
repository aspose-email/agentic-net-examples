using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the source HTML file and the target EML file
            string htmlFilePath = "message.html";
            string emlFilePath = "message.eml";

            // Verify that the source HTML file exists
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(htmlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{htmlFilePath}' does not exist.");
                return;
            }

            // Load the HTML message into a MailMessage object
            using (MailMessage mailMessage = MailMessage.Load(htmlFilePath))
            {
                // Create EML save options (default options are sufficient to keep attachments)
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);

                // Save the message as an EML file; embedded images are retained as attachments
                mailMessage.Save(emlFilePath, emlSaveOptions);
            }

            Console.WriteLine($"HTML message successfully converted to EML: {emlFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
