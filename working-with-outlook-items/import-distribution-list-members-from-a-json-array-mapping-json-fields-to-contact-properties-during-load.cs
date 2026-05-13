using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the JSON file containing distribution list members
            string jsonPath = "members.json";

            // Ensure the JSON file exists; create a minimal placeholder if missing
            if (!File.Exists(jsonPath))
            {
                try
                {
                    File.WriteAllText(jsonPath, "[]");
                    Console.WriteLine($"Placeholder JSON created at '{jsonPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder JSON: {ex.Message}");
                }
                return;
            }

            // Read JSON content
            string jsonContent;
            try
            {
                jsonContent = File.ReadAllText(jsonPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read JSON file: {ex.Message}");
                return;
            }

            // Deserialize JSON array into a list of helper objects
            List<JsonMember> jsonMembers;
            try
            {
                jsonMembers = JsonSerializer.Deserialize<List<JsonMember>>(jsonContent);
                if (jsonMembers == null)
                {
                    Console.Error.WriteLine("JSON deserialization returned null.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to parse JSON: {ex.Message}");
                return;
            }

            // Prepare MAPI distribution list members collection
            MapiDistributionListMemberCollection memberCollection = new MapiDistributionListMemberCollection();

            foreach (JsonMember jsonMember in jsonMembers)
            {
                // Map JSON fields to MAPI member properties
                string displayName = jsonMember.DisplayName ?? string.Empty;
                string emailAddress = jsonMember.EmailAddress ?? string.Empty;

                MapiDistributionListMember member = new MapiDistributionListMember(displayName, emailAddress);

                if (!string.IsNullOrEmpty(jsonMember.AddressType))
                {
                    member.AddressType = jsonMember.AddressType;
                }

                memberCollection.Add(member);
            }

            // Create the distribution list with the populated members
            using (MapiDistributionList distributionList = new MapiDistributionList("Imported Distribution List", memberCollection))
            {
                string outputPath = "ImportedDistributionList.msg";

                // Ensure the output directory exists
                try
                {
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                    return;
                }

                // Save the distribution list to an MSG file
                try
                {
                    distributionList.Save(outputPath);
                    Console.WriteLine($"Distribution list saved to '{outputPath}'.");
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

    // Helper class for JSON deserialization
    private class JsonMember
    {
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressType { get; set; }
    }
}
