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
            // Input MSG file path
            string inputMsgPath = "sample.msg";
            // Output HTML file path
            string outputHtmlPath = "output.html";
            // Folder to save extracted attachments
            string attachmentsFolder = "Attachments";

            // Verify that the input MSG file exists
            if (!File.Exists(inputMsgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputMsgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Ensure the attachments folder exists
            try
            {
                if (!Directory.Exists(attachmentsFolder))
                {
                    Directory.CreateDirectory(attachmentsFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create attachments folder: {ex.Message}");
                return;
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Convert the MapiMessage to a MailMessage preserving formatting
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Save the MailMessage as HTML with embedded resources
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                    htmlOptions.ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml;
                    mailMessage.Save(outputHtmlPath, htmlOptions);
                }

                // Extract and save each attachment
                foreach (MapiAttachment attachment in mapiMessage.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentsFolder, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
