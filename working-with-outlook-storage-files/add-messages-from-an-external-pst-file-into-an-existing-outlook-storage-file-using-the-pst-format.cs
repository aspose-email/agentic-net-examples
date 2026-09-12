using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths to the source PST (to be added) and the target PST (existing Outlook storage)
            string sourcePstPath = "source.pst";
            string targetPstPath = "target.pst";

            // Verify source PST exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Source PST file not found: {sourcePstPath}");
                return;
            }

            // Ensure the target PST exists; create an empty one if it does not
            if (!File.Exists(targetPstPath))
            {
                // Create a new PST file with Unicode format
                using (PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode))
                {
                    // Empty PST created
                }
            }

            // Open the target PST for modification
            using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
            {
                // Merge the external PST into the target PST
                targetPst.MergeWith(new string[] { sourcePstPath });

                Console.WriteLine("Messages from the external PST have been added to the target PST successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
