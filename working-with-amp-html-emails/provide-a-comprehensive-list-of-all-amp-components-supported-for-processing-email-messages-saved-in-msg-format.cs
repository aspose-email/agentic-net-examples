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
            // Define the output MSG file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "AmpComponentsList.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // List of known AMP component type names (as strings)
            // NOTE: The actual Aspose.Email.Amp component classes should be referenced here.
            // This placeholder list represents the supported components as of the current documentation.
            string[] ampComponentNames = new string[]
            {
                "AmpAccordion",
                "AmpAnim",
                "AmpCarousel",
                "AmpFitText",
                "AmpForm",
                "AmpImage",
                "AmpTimeago",
                "AmpList",
                "AmpMap",
                "AmpQuote",
                "AmpSidebar",
                "AmpSocial",
                "AmpTabs",
                "AmpVideo"
            };

            // Create an AMP message (inherits from MailMessage)
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.Subject = "Supported AMP Components";
                ampMessage.Body = "The following AMP components are supported:\n" + string.Join("\n", ampComponentNames);
                // Save the message as MSG
                ampMessage.Save(outputPath, SaveOptions.DefaultMsg);
            }

            Console.WriteLine("AMP components list saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
