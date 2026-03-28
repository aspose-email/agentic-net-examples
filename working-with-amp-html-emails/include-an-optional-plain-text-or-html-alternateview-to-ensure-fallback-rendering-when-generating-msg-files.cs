using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output directory and file path
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "ampMessage.msg");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email Example";

                // Create plain‑text alternate view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text version.", null, "text/plain"))
                {
                    // Create HTML alternate view
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "<html><body><h1>Hello AMP</h1></body></html>", null, "text/html"))
                    {
                        // Add alternate views to the message
                        ampMessage.AlternateViews.Add(plainView);
                        ampMessage.AlternateViews.Add(htmlView);

                        // Save the message as MSG with Unicode support
                        ampMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
