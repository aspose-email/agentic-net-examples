using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output VCF file path
            string outputPath = "distributionList.vcf";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new distribution list
            using (MapiDistributionList distributionList = new MapiDistributionList())
            {
                distributionList.DisplayName = "Sample Distribution List";

                // Add members to the distribution list
                distributionList.Members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Jane Smith", "jane.smith@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Bob Johnson", "bob.johnson@example.com"));

                // Save the distribution list as a VCF file (vCard format)
                MapiDistributionListSaveOptions saveOptions = new MapiDistributionListSaveOptions(ContactSaveFormat.VCard);
                distributionList.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"Distribution list exported successfully to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
