using Aspose.Email;
using Aspose.Email.Storage.Olm;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the OLM file
            string olmFilePath = "sample.olm";

            // Verify that the OLM file exists
            if (!File.Exists(olmFilePath))
            {
                Console.Error.WriteLine($"The OLM file '{olmFilePath}' does not exist.");
                return;
            }

            // Open the OLM storage
            using (OlmStorage olm = OlmStorage.FromFile(olmFilePath))
            {
                IList<OlmItemCategory> categories;
                try
                {
                    categories = olm.GetCategories();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve categories from OLM: {ex.Message}");
                    return;
                }

                if (categories == null || categories.Count == 0)
                {
                    Console.WriteLine("No categories found in the OLM file.");
                    return;
                }

                // In a real scenario, you would store these in a database.
                // For this example, we simply output them to the console.
                foreach (OlmItemCategory category in categories)
                {
                    string colorString = category.Color != null ? category.Color.ToString() : string.Empty;
                    Console.WriteLine($"Category: {category.Name ?? string.Empty}, Color: {colorString}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
