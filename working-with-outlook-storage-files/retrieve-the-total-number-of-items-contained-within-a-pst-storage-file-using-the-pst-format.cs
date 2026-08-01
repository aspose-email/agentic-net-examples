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
            const string pstPath = "storage.pst";

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                // Create an empty PST with Unicode format
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the message store and get the total items count
                MessageStore store = pst.Store;
                int totalItemsCount = store.GetTotalItemsCount();

                // Output the result
                Console.WriteLine($"Total items count: {totalItemsCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
