using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputEmlPath = "output.eml";

            // Verify that the input HTML file exists
            if (!File.Exists(inputHtmlPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputHtmlPath}");
                return;
            }

            // Load the HTML file into a MailMessage
            Aspose.Email.MailMessage mailMessage = null;
            try
            {
                mailMessage = Aspose.Email.MailMessage.Load(inputHtmlPath, new Aspose.Email.HtmlLoadOptions());
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load HTML file: {loadEx.Message}");
                return;
            }

            // Ensure the MailMessage is disposed after use
            using (mailMessage)
            {
                // Configure EML save options to preserve embedded message format
                Aspose.Email.EmlSaveOptions emlSaveOptions = new Aspose.Email.EmlSaveOptions(Aspose.Email.MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Save the MailMessage as an EML file
                try
                {
                    mailMessage.Save(outputEmlPath, emlSaveOptions);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save EML file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}