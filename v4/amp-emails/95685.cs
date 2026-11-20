using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "amp_header_accordion.eml";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create an AMP message
            AmpMessage ampMessage = new AmpMessage();

            // Set the HTML body with a header element
            ampMessage.HtmlBody = "<h1>Welcome to AMP Email</h1>";

            // Create an accordion component
            AmpAccordion accordion = new AmpAccordion();

            // First panel
            Section panel1 = new Section();
            panel1.Header = new SectionHeader(SectionHeaderType.h2, "Panel 1");
            panel1.Value = new SectionValue("<p>Content for panel 1.</p>");

            // Second panel
            Section panel2 = new Section();
            panel2.Header = new SectionHeader(SectionHeaderType.h2, "Panel 2");
            panel2.Value = new SectionValue("<p>Content for panel 2.</p>");

            // Add panels to the accordion
            accordion.Sections.Add(panel1);
            accordion.Sections.Add(panel2);

            // Add the accordion to the AMP body of the message
            ampMessage.AddAmpComponent(accordion);

            // Save the AMP message to an .eml file
            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                ampMessage.Save(fileStream);
            }

            Console.WriteLine("AMP email saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
