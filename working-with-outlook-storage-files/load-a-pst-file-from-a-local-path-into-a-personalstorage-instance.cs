using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file as a placeholder.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {createEx.Message}");
                    return;
                }
            }

            // Load the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Example operation: output total items count.
                int totalItems = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items in PST: {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
