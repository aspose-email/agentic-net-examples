using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Olm;

class Program
{
    static void Main()
    {
        try
        {
            string olmFilePath = "source.olm";
            string pstFilePath = "target.pst";

            // Create placeholder files if they do not exist
            if (!File.Exists(olmFilePath))
            {
                File.WriteAllBytes(olmFilePath, new byte[0]); // placeholder OLM file
            }

            if (!File.Exists(pstFilePath))
            {
                // Create an empty PST file as a placeholder
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // No folders or items needed for placeholder
                }
            }

            // Load OLM categories
            IList<OlmItemCategory> olmCategories;
            try
            {
                using (OlmStorage olmStorage = OlmStorage.FromFile(olmFilePath))
                {
                    olmCategories = olmStorage.GetCategories();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load OLM file: {ex.Message}");
                return;
            }

            // Load PST categories
            IList<PstItemCategory> pstCategories;
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    pstCategories = pst.GetCategories();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
                return;
            }

            // Compare categories by name and color
            foreach (OlmItemCategory olmCategory in olmCategories)
            {
                PstItemCategory matchingPstCategory = pstCategories.FirstOrDefault(c => c.Name == olmCategory.Name);
                if (matchingPstCategory != null)
                {
                    // OlmItemCategory.Color may be a string; PST uses OutlookCategoryColor enum
                    bool colorsMatch = matchingPstCategory.Color.ToString().Equals(olmCategory.Color, StringComparison.OrdinalIgnoreCase);
                    Console.WriteLine($"Category '{olmCategory.Name}': OLM Color = {olmCategory.Color}, PST Color = {matchingPstCategory.Color} => {(colorsMatch ? "Match" : "Mismatch")}");
                }
                else
                {
                    Console.WriteLine($"Category '{olmCategory.Name}' exists in OLM but not in PST.");
                }
            }

            // Report PST categories that are not present in OLM
            foreach (PstItemCategory pstCategory in pstCategories)
            {
                bool existsInOlm = olmCategories.Any(c => c.Name == pstCategory.Name);
                if (!existsInOlm)
                {
                    Console.WriteLine($"Category '{pstCategory.Name}' exists in PST but not in OLM.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
