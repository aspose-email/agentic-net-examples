using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            string htmlPath = "input.html";
            // Folder that contains resources (e.g., images) referenced by the HTML
            string resourcesFolder = "Resources";

            // Guard against missing input file
            if (!File.Exists(htmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(htmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input HTML file not found: {htmlPath}");
                return;
            }

            // Ensure the resources folder exists (create empty if missing)
            if (!Directory.Exists(resourcesFolder))
            {
                Directory.CreateDirectory(resourcesFolder);
            }

            // Load the HTML message with options to resolve resources from the folder
            HtmlLoadOptions loadOptions = new HtmlLoadOptions
            {
                PreferredTextEncoding = System.Text.Encoding.UTF8,
                ShouldAddPlainTextView = true,
                PathToResources = resourcesFolder
            };

            using (MailMessage message = MailMessage.Load(htmlPath, loadOptions))
            {
                // Prepare save options to preserve embedded resources as attachments
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Output EML file path
                string emlPath = "output.eml";

                // Save the message as EML with embedded images retained
                message.Save(emlPath, saveOptions);
                Console.WriteLine($"HTML message successfully converted to EML: {emlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
