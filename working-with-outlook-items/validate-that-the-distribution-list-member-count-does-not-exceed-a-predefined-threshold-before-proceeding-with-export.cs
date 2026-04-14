using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths
            string inputFile = "distributionList.msg";
            string outputFile = "exportedDistributionList.msg";

            // Verify input file exists
            if (!File.Exists(inputFile))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputFile}' does not exist.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the MAPI message
            using (MapiMessage msg = MapiMessage.Load(inputFile))
            {
                // Check if the message is a distribution list
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The provided message is not a distribution list.");
                    return;
                }

                // Convert to MapiDistributionList
                MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem();

                // Define the maximum allowed members
                const int maxMembers = 100;

                // Validate member count
                int memberCount = distList.Members.Count;
                if (memberCount > maxMembers)
                {
                    Console.Error.WriteLine($"Distribution list has {memberCount} members, which exceeds the allowed maximum of {maxMembers}.");
                    return;
                }

                // Export (save) the distribution list
                try
                {
                    distList.Save(outputFile);
                    Console.WriteLine($"Distribution list exported successfully to '{outputFile}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save distribution list: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
