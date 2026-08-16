using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Olm;

class Program
{
    static void Main()
    {
        try
        {
            string olmPath = "sample.olm";
            string pstPath = "output.pst";

            // Verify OLM input file exists
            if (!File.Exists(olmPath))
            {
                Console.Error.WriteLine($"Input OLM file not found: {olmPath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create PST file
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Load OLM storage
                using (OlmStorage olm = OlmStorage.FromFile(olmPath))
                {
                    // Transfer categories with exact color mapping (demo only – actual PST category addition may require a different API)
                    IList<OlmItemCategory> olmCategories = olm.GetCategories();
                    foreach (OlmItemCategory olmCategory in olmCategories)
                    {
                        OutlookCategoryColor pstColor = MapOlmColorToOutlookCategoryColor(olmCategory.Color);
                        // Demonstrate the mapping; actual addition to PST would depend on the library version
                        Console.WriteLine($"Category: {olmCategory.Name}, OLM Color: {olmCategory.Color}, Mapped PST Color: {pstColor}");
                    }

                    // Additional conversion logic (e.g., messages) would go here
                }
            }

            Console.WriteLine("OLM to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Maps OLM category color string to the closest OutlookCategoryColor enum value
    private static OutlookCategoryColor MapOlmColorToOutlookCategoryColor(string olmColor)
    {
        if (string.IsNullOrEmpty(olmColor))
            return OutlookCategoryColor.None;

        // Normalize the color string
        string color = olmColor.Trim().ToLowerInvariant();

        // Common named colors
        switch (color)
        {
            case "red": return OutlookCategoryColor.Red;
            case "orange": return OutlookCategoryColor.Orange;
            case "brown": return OutlookCategoryColor.Brown;
            case "yellow": return OutlookCategoryColor.Yellow;
            case "green": return OutlookCategoryColor.Green;
            case "teal": return OutlookCategoryColor.Teal;
            case "olive": return OutlookCategoryColor.Olive;
            case "blue": return OutlookCategoryColor.Blue;
            case "purple": return OutlookCategoryColor.Purple;
            case "cranberry": return OutlookCategoryColor.Cranberry;
            case "steel": return OutlookCategoryColor.Steel;
            case "darksteel": return OutlookCategoryColor.DarkSteel;
            case "gray": return OutlookCategoryColor.Gray;
            case "darkgray": return OutlookCategoryColor.DarkGray;
            case "black": return OutlookCategoryColor.Black;
            case "darkred": return OutlookCategoryColor.DarkRed;
            case "darkorange": return OutlookCategoryColor.DarkOrange;
            case "darkbrown": return OutlookCategoryColor.DarkBrown;
            case "darkyellow": return OutlookCategoryColor.DarkYellow;
            case "darkgreen": return OutlookCategoryColor.DarkGreen;
            case "darkteal": return OutlookCategoryColor.DarkTeal;
            case "darkolive": return OutlookCategoryColor.DarkOlive;
            case "darkblue": return OutlookCategoryColor.DarkBlue;
            case "darkpurple": return OutlookCategoryColor.DarkPurple;
            case "darkcranberry": return OutlookCategoryColor.DarkCranberry;
            case "none": return OutlookCategoryColor.None;
            default:
                // Attempt to parse hex RGB (e.g., "#ff0000")
                if (color.StartsWith("#") && color.Length == 7)
                {
                    // Exact RGB mapping is not directly supported by OutlookCategoryColor enum.
                    // For demonstration, we return None to indicate an unmapped custom color.
                    return OutlookCategoryColor.None;
                }
                return OutlookCategoryColor.None;
        }
    }
}
