using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "amp_email.eml";

            // Ensure the directory exists
            try
            {
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create an AMP accordion with two sections
            AmpAccordion accordion = new AmpAccordion();

            // First section
            Section section1 = new Section
            {
                Header = new SectionHeader((SectionHeaderType)0, "First Section"),
                Value = new SectionValue("<p>This is the <strong>first</strong> section content.</p>")
            };

            // Second section
            Section section2 = new Section
            {
                Header = new SectionHeader((SectionHeaderType)0, "Second Section"),
                Value = new SectionValue("<p>This is the <em>second</em> section content.</p>")
            };

            // Add sections to the accordion
            accordion.Sections.Add(section1);
            accordion.Sections.Add(section2);

            // Create an AMP message and embed the accordion
            using (AmpMessage message = new AmpMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "AMP Email with Accordion";

                // Set the AMP HTML body using the accordion's AMP representation
                message.AmpHtmlBody = accordion.ToAmpHtml();

                // Provide a fallback HTML body for non‑AMP clients
                message.HtmlBody = "<p>This email contains interactive AMP content. Please view it in a compatible email client.</p>";

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"AMP email saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save the message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
