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
            string outputPath = "mailinglist.txt";

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
                // Check if the message is a distribution list
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                    return;
                }

                // Convert to MapiDistributionList
                using (MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem())
                {
                    // Write each member's email address to the output file
                    using (StreamWriter writer = new StreamWriter(outputPath))
                    {
                        foreach (MapiDistributionListMember member in distList.Members)
                        {
                            if (!string.IsNullOrEmpty(member.EmailAddress))
                            {
                                writer.WriteLine(member.EmailAddress);
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Mailing list saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
