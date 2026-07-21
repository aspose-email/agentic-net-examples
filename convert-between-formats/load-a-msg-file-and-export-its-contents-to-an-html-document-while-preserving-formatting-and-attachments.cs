using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: This sample loads a MSG file and saves it as an HTML document with embedded resources.
            string inputMsgPath = "sample.msg";
            string outputHtmlPath = "sample.html";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the Outlook MSG file
            MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

            // Convert to MailMessage for HTML export
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions);

            // Export to HTML with embedded attachments
            using (mailMsg)
            {
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };
                mailMsg.Save(outputHtmlPath, htmlOptions);
            }

            Console.WriteLine($"MSG file successfully exported to HTML: {outputHtmlPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
