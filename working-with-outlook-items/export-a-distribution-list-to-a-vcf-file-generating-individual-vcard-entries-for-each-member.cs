using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string outputDirectory = "output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a distribution list and add members
            using (MapiDistributionList distributionList = new MapiDistributionList())
            {
                distributionList.DisplayName = "Sample Distribution List";

                distributionList.Members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));
                distributionList.Members.Add(new MapiDistributionListMember("Jane Smith", "jane.smith@example.com"));

                int memberIndex = 1;
                foreach (MapiDistributionListMember member in distributionList.Members)
                {
                    // Build a simple vCard (VERSION 3.0) manually
                    string vcardContent = $"BEGIN:VCARD\r\nVERSION:3.0\r\nFN:{member.DisplayName}\r\nEMAIL:{member.EmailAddress}\r\nEND:VCARD\r\n";

                    string vcfPath = Path.Combine(outputDirectory, $"member{memberIndex}.vcf");
                    try
                    {
                        File.WriteAllText(vcfPath, vcardContent);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save vCard for {member.DisplayName}: {ex.Message}");
                    }

                    memberIndex++;
                }
            }

            Console.WriteLine("Distribution list exported to individual vCard files.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
