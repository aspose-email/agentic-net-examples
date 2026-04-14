using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "distributionList.msg";
            string outputPath = "renamedDistributionList.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Check that the MSG contains a distribution list
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The MSG file does not contain a distribution list.");
                    return;
                }

                // Convert to MapiDistributionList
                using (MapiDistributionList distributionList = (MapiDistributionList)msg.ToMapiMessageItem())
                {
                    // Rename according to new naming convention
                    distributionList.DisplayName = "New Distribution List Name";

                    // Save the updated distribution list to a new MSG file
                    distributionList.Save(outputPath);
                    Console.WriteLine($"Distribution list saved to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
