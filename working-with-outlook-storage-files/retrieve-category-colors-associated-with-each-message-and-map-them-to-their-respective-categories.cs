using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine("Placeholder PST file created at: " + pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder PST: " + ex.Message);
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve all PST categories and map their names to colors.
                List<PstItemCategory> pstCategories = pst.GetCategories();
                Dictionary<string, OutlookCategoryColor> categoryColorMap = new Dictionary<string, OutlookCategoryColor>(StringComparer.OrdinalIgnoreCase);
                foreach (PstItemCategory cat in pstCategories)
                {
                    if (!categoryColorMap.ContainsKey(cat.Name))
                    {
                        categoryColorMap[cat.Name] = cat.Color;
                    }
                }

                // Iterate through each folder and each message.
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        using (MapiMessage message = pst.ExtractMessage(msgInfo))
                        {
                            // Get category names assigned to this message.
                            IList<string> messageCategories = FollowUpManager.GetCategories(message);

                            Console.WriteLine($"Message Subject: {message.Subject}");
                            if (messageCategories.Count == 0)
                            {
                                Console.WriteLine("  No categories assigned.");
                            }
                            else
                            {
                                foreach (string catName in messageCategories)
                                {
                                    if (categoryColorMap.TryGetValue(catName, out OutlookCategoryColor color))
                                    {
                                        Console.WriteLine($"  Category: {catName}, Color: {color}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"  Category: {catName}, Color: (unknown)");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
