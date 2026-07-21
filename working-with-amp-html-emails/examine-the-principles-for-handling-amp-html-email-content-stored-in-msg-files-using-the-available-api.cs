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
            // Define output file path
            string outputPath = "amp_email.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Basic mail properties
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email Example";

                // Create an AMP image component (width, height)
                AmpImage ampImage = new AmpImage(300, 200);
                ampImage.Src = "https://example.com/image.png";
                ampImage.Alt = "Example image";

                // Add the image component to the message
                ampMessage.AddAmpComponent(ampImage);

                // Create an AMP fit text component
                AmpFitText ampFitText = new AmpFitText("Hello, AMP world!");
                // Add the text component to the message
                ampMessage.AddAmpComponent(ampFitText);

                // Save the AMP message as a MSG file
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fileStream, SaveOptions.DefaultMsgUnicode);
                }

                Console.WriteLine("AMP email saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
