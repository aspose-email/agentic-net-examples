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
            // Input OLM file and output PST file paths
            string olmPath = "input.olm";
            string pstPath = "output.pst";

            // Guard OLM file existence
            if (!File.Exists(olmPath))
            {
                Console.Error.WriteLine($"OLM file not found: {olmPath}");
                return;
            }

            // Ensure the directory for the PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Load OLM storage
            using (OlmStorage olm = OlmStorage.FromFile(olmPath))
            {
                // Retrieve categories from OLM
                IList<OlmItemCategory> olmCategories = olm.GetCategories();

                // Open existing PST or create a new one
                PersonalStorage pst;
                if (File.Exists(pstPath))
                {
                    // Open for writing
                    pst = PersonalStorage.FromFile(pstPath, true);
                }
                else
                {
                    // Create a new Unicode PST
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }

                using (pst)
                {
                    // NOTE: Aspose.Email PST API does not expose a direct method to add categories.
                    // This sample demonstrates how to map OLM category colors to PST OutlookCategoryColor enum.
                    // If a future API provides AddCategory, replace the placeholder loop with that call.

                    foreach (OlmItemCategory olmCat in olmCategories)
                    {
                        string catName = olmCat.Name ?? "Unnamed";
                        string olmColorString = olmCat.Color ?? string.Empty;

                        // Attempt to map the OLM color string to OutlookCategoryColor enum
                        OutlookCategoryColor pstColor = OutlookCategoryColor.None;
                        if (!string.IsNullOrEmpty(olmColorString))
                        {
                            // The OLM color is a string representation (e.g., "Red", "Blue").
                            // Try to parse it case‑insensitively.
                            if (Enum.TryParse(typeof(OutlookCategoryColor), olmColorString, true, out object parsed))
                            {
                                pstColor = (OutlookCategoryColor)parsed;
                            }
                        }

                        // Create PST category instance (placeholder – actual addition to PST not supported in current API)
                        PstItemCategory pstCategory = new PstItemCategory(catName, pstColor);

                        // Placeholder: output mapping information
                        Console.WriteLine($"Mapped OLM category '{catName}' with color '{olmColorString}' to PST color '{pstColor}'.");
                        // If a future method like pst.AddCategory(pstCategory) becomes available, invoke it here.
                    }

                    // Since no direct PST category addition is available, we simply save the PST (if newly created)
                    // The PST is already open for writing; disposing it will persist changes.
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
