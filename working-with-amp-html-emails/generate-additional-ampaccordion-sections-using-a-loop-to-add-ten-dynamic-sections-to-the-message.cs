using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "AmpMessage.eml";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Build AMP HTML with an accordion containing ten dynamic sections
            StringBuilder ampBuilder = new StringBuilder();
            ampBuilder.AppendLine("<!doctype html>");
            ampBuilder.AppendLine("<html amp>");
            ampBuilder.AppendLine("<head>");
            ampBuilder.AppendLine("  <meta charset='utf-8'>");
            ampBuilder.AppendLine("  <script async src='https://cdn.ampproject.org/v0.js'></script>");
            ampBuilder.AppendLine("  <script async custom-element='amp-accordion' src='https://cdn.ampproject.org/v0/amp-accordion-0.1.js'></script>");
            ampBuilder.AppendLine("</head>");
            ampBuilder.AppendLine("<body>");
            ampBuilder.AppendLine("  <amp-accordion>");

            for (int i = 1; i <= 10; i++)
            {
                ampBuilder.AppendLine("    <section>");
                ampBuilder.AppendLine($"      <h4>Section {i}</h4>");
                ampBuilder.AppendLine($"      <p>Content for section {i}.</p>");
                ampBuilder.AppendLine("    </section>");
            }

            ampBuilder.AppendLine("  </amp-accordion>");
            ampBuilder.AppendLine("</body>");
            ampBuilder.AppendLine("</html>");

            // Create the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email with Dynamic Accordion";
                ampMessage.AmpHtmlBody = ampBuilder.ToString();

                // Save the message to a file
                try
                {
                    ampMessage.Save(outputPath);
                    Console.WriteLine($"AMP message saved to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save AMP message: {saveEx.Message}");
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
