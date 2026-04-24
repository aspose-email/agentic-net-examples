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
            // Prepare output file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "amp_message.eml");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a placeholder attachment file if it does not exist
            string attachmentPath = Path.Combine(Environment.CurrentDirectory, "attachment.txt");
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Create the AmpMessage and add components
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic properties
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AmpMessage with components";
                ampMessage.Body = "This is the plain text body.";
                ampMessage.IsBodyHtml = false;

                // Add an alternate view (HTML)
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString("<html><body><h1>Hello</h1></body></html>", System.Text.Encoding.UTF8, "text/html");
                ampMessage.AddAlternateView(htmlView);

                // Add an attachment
                Attachment attachment = new Attachment(attachmentPath);
                ampMessage.AddAttachment(attachment);

                // Add a linked resource
                LinkedResource linkedResource = new LinkedResource(attachmentPath);
                ampMessage.LinkedResources.Add(linkedResource);

                // Log total number of components before serialization
                int totalComponents = ampMessage.AlternateViews.Count + ampMessage.Attachments.Count + ampMessage.LinkedResources.Count;
                Console.WriteLine($"Total components in AmpMessage: {totalComponents}");

                // Save the message to a file
                try
                {
                    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        ampMessage.Save(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save AmpMessage: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
