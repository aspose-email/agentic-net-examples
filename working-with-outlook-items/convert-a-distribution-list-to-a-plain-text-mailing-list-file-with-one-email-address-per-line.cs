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
            string inputPath = "distributionList.msg";
            string outputPath = "mailinglist.txt";

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

            string outputDir = Path.GetDirectoryName(outputPath);
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

            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    if (msg.SupportedType != MapiItemType.DistList)
                    {
                        Console.Error.WriteLine("The provided file is not a distribution list.");
                        return;
                    }

                    using (MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem())
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath, false))
                        {
                            foreach (MapiDistributionListMember member in distList.Members)
                            {
                                writer.WriteLine(member.EmailAddress);
                            }
                        }
                    }
                }

                Console.WriteLine($"Mailing list saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing distribution list: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
