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

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at: {pstPath}");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the message store to check password protection.
                MessageStore store = pst.Store;
                bool isProtected = store.IsPasswordProtected;

                Console.WriteLine($"PST file \"{pstPath}\" password protected: {isProtected}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
