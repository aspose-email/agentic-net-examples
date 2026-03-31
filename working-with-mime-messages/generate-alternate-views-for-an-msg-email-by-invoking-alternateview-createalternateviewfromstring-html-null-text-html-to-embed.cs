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
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample with Alternate Views";

                // Create a plain‑text alternate view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is plain text content.", null, "text/plain");

                // Create an HTML alternate view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>Hello, World!</h1></body></html>", null, "text/html");

                // Add the alternate views to the message
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Save the message to an MSG file
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
