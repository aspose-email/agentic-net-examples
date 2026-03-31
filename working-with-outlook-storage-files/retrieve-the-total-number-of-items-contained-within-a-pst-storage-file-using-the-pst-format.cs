using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Placeholder PST created; no additional content needed
                }
                Console.WriteLine($"Placeholder PST created at {pstPath}");
            }

            // Open the PST file and retrieve the total items count
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                int totalItemsCount = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
