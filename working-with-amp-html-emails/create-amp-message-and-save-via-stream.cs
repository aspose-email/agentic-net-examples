using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "AmpMessage.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                    return;
                }
            }

            // Create and configure the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.Subject = "Sample AMP Email";
                ampMessage.Body = "This is the plain text body.";
                ampMessage.IsBodyHtml = true;
                ampMessage.HtmlBody = "<p>This is the HTML body.</p>";
                ampMessage.AmpHtmlBody = "<amp-html><p>This is the AMP HTML body.</p></amp-html>";

                // Save the message as MSG using a stream and appropriate SaveOptions
                try
                {
                    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        SaveOptions saveOptions = SaveOptions.CreateSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                        ampMessage.Save(fileStream, saveOptions);
                    }
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save AMP message: {saveEx.Message}");
                    return;
                }
            }

            Console.WriteLine($"AMP message saved successfully to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}