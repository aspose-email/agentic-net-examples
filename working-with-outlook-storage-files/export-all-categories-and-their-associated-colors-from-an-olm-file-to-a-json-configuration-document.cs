using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email.Storage.Olm;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.olm";
            string outputPath = "categories.json";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Load OLM storage and extract categories
            try
            {
                using (OlmStorage olm = OlmStorage.FromFile(inputPath))
                {
                    IList<OlmItemCategory> olmCategories = olm.GetCategories();

                    var categoryList = new List<CategoryInfo>();
                    foreach (OlmItemCategory olmCategory in olmCategories)
                    {
                        var info = new CategoryInfo
                        {
                            Name = olmCategory.Name,
                            Color = olmCategory.Color
                        };
                        categoryList.Add(info);
                    }

                    // Serialize to JSON
                    string json = JsonSerializer.Serialize(categoryList, new JsonSerializerOptions { WriteIndented = true });

                    // Write JSON to file
                    try
                    {
                        File.WriteAllText(outputPath, json);
                        Console.WriteLine($"Categories exported to {outputPath}");
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {writeEx.Message}");
                    }
                }
            }
            catch (Exception olmEx)
            {
                Console.Error.WriteLine($"Failed to process OLM file: {olmEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple DTO for JSON serialization
    private class CategoryInfo
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
