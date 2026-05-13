using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email.Storage.Olm;

namespace OlmCategoryExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input OLM file path (adjust as needed)
                string olmFilePath = "sample.olm";

                // Verify input file exists
                if (!File.Exists(olmFilePath))
                {
                    Console.Error.WriteLine($"Input OLM file not found: {olmFilePath}");
                    return;
                }

                // Load OLM storage
                using (OlmStorage olm = OlmStorage.FromFile(olmFilePath))
                {
                    // Retrieve categories
                    IList<OlmItemCategory> olmCategories = olm.GetCategories();

                    // Prepare list for JSON serialization
                    List<CategoryInfo> categoryList = new List<CategoryInfo>();

                    foreach (OlmItemCategory olmCategory in olmCategories)
                    {
                        CategoryInfo info = new CategoryInfo();
                        info.Name = olmCategory.Name;
                        // Convert color to string representation
                        info.Color = olmCategory.Color != null ? olmCategory.Color.ToString() : string.Empty;
                        categoryList.Add(info);
                    }

                    // Serialize to JSON
                    string json = JsonSerializer.Serialize(categoryList, new JsonSerializerOptions { WriteIndented = true });

                    // Output JSON file path
                    string jsonFilePath = "categories.json";

                    // Ensure output directory exists
                    string outputDirectory = Path.GetDirectoryName(jsonFilePath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Write JSON to file
                    try
                    {
                        File.WriteAllText(jsonFilePath, json);
                        Console.WriteLine($"Category data saved to {jsonFilePath}");
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {writeEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    // Simple DTO for JSON output
    public class CategoryInfo
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
