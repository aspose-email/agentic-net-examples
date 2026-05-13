using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstFilePath = "sample.pst";
            string csvFilePath = "categories.csv";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output directory exists
            string csvDirectory = Path.GetDirectoryName(csvFilePath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                try
                {
                    Directory.CreateDirectory(csvDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory for CSV output: {dirEx.Message}");
                    return;
                }
            }

            // Open PST and retrieve categories
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    List<PstItemCategory> categories = pst.GetCategories();

                    // Write categories to CSV
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(csvFilePath, false))
                        {
                            writer.WriteLine("Name,Color");
                            foreach (PstItemCategory category in categories)
                            {
                                string line = $"{category.Name},{category.Color}";
                                writer.WriteLine(line);
                            }
                        }
                        Console.WriteLine($"Categories exported to {csvFilePath}");
                    }
                    catch (Exception writeEx)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {writeEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
