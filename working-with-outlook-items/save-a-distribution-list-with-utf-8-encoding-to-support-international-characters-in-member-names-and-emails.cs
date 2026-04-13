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
            string outputPath = "DistributionList.vcf";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a collection of members with international characters
            MapiDistributionListMemberCollection members = new MapiDistributionListMemberCollection();
            members.Add(new MapiDistributionListMember("张伟", "zhangwei@example.com"));
            members.Add(new MapiDistributionListMember("Иван Иванов", "ivan.ivanov@example.com"));
            members.Add(new MapiDistributionListMember("علي الأحمد", "ali.ahmad@example.com"));

            using (MapiDistributionList distributionList = new MapiDistributionList("国际分发列表", members))
            {
                // Save the distribution list as VCard (UTF‑8 encoded)
                MapiDistributionListSaveOptions saveOptions = new MapiDistributionListSaveOptions(ContactSaveFormat.VCard);
                distributionList.Save(outputPath, saveOptions);
                Console.WriteLine($"Distribution list saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
