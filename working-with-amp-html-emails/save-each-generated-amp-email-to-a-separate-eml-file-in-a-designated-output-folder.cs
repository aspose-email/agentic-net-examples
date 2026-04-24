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
            string outputFolder = "OutputAmpEmails";

            // Ensure the output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create and save three AMP messages
            for (int i = 1; i <= 3; i++)
            {
                string filePath = Path.Combine(outputFolder, $"AmpMessage{i}.eml");

                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.From = "sender@example.com";
                    ampMessage.To.Add("recipient@example.com");
                    ampMessage.Subject = $"AMP Email {i}";
                    ampMessage.Body = "This is a plain text body.";
                    ampMessage.IsBodyHtml = true;
                    ampMessage.HtmlBody = $"<html><body><h1>AMP Email {i}</h1></body></html>";

                    // Save the AMP message to an .eml file
                    try
                    {
                        ampMessage.Save(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save AMP message to '{filePath}': {ex.Message}");
                        return;
                    }
                }
            }

            Console.WriteLine("AMP messages saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
