using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "input.msg";
            string imagePath = "image.jpg";
            string outputPath = "output.msg";

            // Verify input files exist
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Load the existing MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Ensure there is at least one alternate view
                if (message.AlternateViews.Count > 0)
                {
                    // Create linked resource and add to the first alternate view
                    LinkedResource linked = new LinkedResource(imagePath);
                    linked.ContentId = "image1";
                    message.AlternateViews[0].LinkedResources.Add(linked);
                }
                else
                {
                    // Create a simple HTML alternate view with the linked resource
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "<html><body><img src='cid:image1'></body></html>",
                        null,
                        "text/html");

                    LinkedResource linked = new LinkedResource(imagePath);
                    linked.ContentId = "image1";
                    htmlView.LinkedResources.Add(linked);
                    message.AlternateViews.Add(htmlView);
                }

                // Save the modified message
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
