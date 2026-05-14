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

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The MSG file does not contain a distribution list.");
                    return;
                }

                using (MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem())
                {
                    // Rename according to the new naming convention
                    distList.DisplayName = "New Naming Convention";

                    // Save the updated distribution list to a new MSG file
                    distList.Save(outputPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
