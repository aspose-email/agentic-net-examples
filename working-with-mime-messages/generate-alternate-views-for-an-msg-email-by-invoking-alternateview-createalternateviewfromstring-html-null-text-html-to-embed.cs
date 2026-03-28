using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputFilePath = "AlternateViewMessage.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MailMessage
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Message with HTML Alternate View";

                // HTML content for the alternate view
                string htmlContent = "<html><body><h1>Hello, World!</h1><p>This is an HTML alternate view.</p></body></html>";

                // Create the HTML alternate view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, null, "text/html");

                // Add the alternate view to the message
                message.AlternateViews.Add(htmlView);

                // Save the message to MSG format
                try
                {
                    message.Save(outputFilePath, SaveOptions.DefaultMsgUnicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save the message: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine($"Message saved successfully to '{outputFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
