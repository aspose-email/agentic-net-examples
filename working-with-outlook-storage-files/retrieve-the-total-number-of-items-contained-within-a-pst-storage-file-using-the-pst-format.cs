using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstItemCount
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file
                string pstPath = "storage.pst";

                // Verify that the PST file exists before attempting to open it
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                // Open the PST file using a using block to ensure proper disposal
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Retrieve the total number of items in the PST storage
                    int totalItemsCount = pst.Store.GetTotalItemsCount();

                    // Output the count to the console
                    Console.WriteLine($"Total items count: {totalItemsCount}");
                }
            }
            catch (Exception ex)
            {
                // Write any unexpected errors to the error console
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
