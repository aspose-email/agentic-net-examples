using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output MSG file path
            string outputPath = "output.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create and configure the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com");
                ampMessage.To.Add(new MailAddress("recipient@example.com"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain‑text fallback body.";
                ampMessage.AmpHtmlBody = "<amp-html><h1>Hello, AMP!</h1></amp-html>";

                // Save the message as an MSG file using a stream and SaveOptions
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fileStream, SaveOptions.DefaultMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
