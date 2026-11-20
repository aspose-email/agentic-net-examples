using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        // Define output file path
        string outputPath = "amp_styled_accordion.eml";

        // Ensure the output directory exists
        try
        {
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
            return;
        }

        // Create and configure the AMP message
        try
        {
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set HTML body with a style block
                ampMessage.HtmlBody = @"<style>
                                            .accordion {font-family: Arial;}
                                            .panel {padding: 10px; border: 1px solid #ccc;}
                                         </style>
                                         <p>This is an AMP email with an accordion.</p>";

                // Create an AMP accordion component
                AmpAccordion accordion = new AmpAccordion();

                // Add three panels to the accordion
                Section panel1 = new Section { Header = new SectionHeader(SectionHeaderType.h2, "Panel 1"), Value = new SectionValue("Content for panel 1.") };
                Section panel2 = new Section { Header = new SectionHeader(SectionHeaderType.h2, "Panel 2"), Value = new SectionValue("Content for panel 2.") };
                Section panel3 = new Section { Header = new SectionHeader(SectionHeaderType.h2, "Panel 3"), Value = new SectionValue("Content for panel 3.") };

                accordion.Sections.Add(panel1);
                accordion.Sections.Add(panel2);
                accordion.Sections.Add(panel3);

                // Add the accordion to the AMP body
                ampMessage.AddAmpComponent(accordion);

                // Save the message as an EML file
                ampMessage.Save(outputPath, SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating or saving AMP message: {ex.Message}");
            return;
        }
    }
}
