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
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "distributionList.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MAPI distribution list
            using (MapiDistributionList distributionList = new MapiDistributionList())
            {
                distributionList.DisplayName = "International Contacts";

                // Add members with Unicode characters in names and emails
                distributionList.Members.Add(new MapiDistributionListMember("Иван Иванов", "ivan@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("张伟", "zhang@example.cn"));
                distributionList.Members.Add(new MapiDistributionListMember("علي الأحمد", "ali@example.sa"));

                // Save the distribution list using default save options (Unicode-aware)
                MapiDistributionListSaveOptions saveOptions = MapiDistributionListSaveOptions.Default;
                distributionList.Save(outputPath, saveOptions);
            }

            Console.WriteLine("Distribution list saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
