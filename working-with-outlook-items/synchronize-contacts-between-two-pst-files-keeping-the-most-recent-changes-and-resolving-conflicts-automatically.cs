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
            string sourcePstPath = "source.pst";
            string targetPstPath = "target.pst";

            // Verify source PST exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Source PST file not found: {sourcePstPath}");
                return;
            }

            // Ensure target PST exists; create a minimal one if missing
            if (!File.Exists(targetPstPath))
            {
                try
                {
                    using (PersonalStorage createdPst = PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode))
                    {
                        // Create default Contacts folder in the new PST
                        createdPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create target PST: {ex.Message}");
                    return;
                }
            }

            // Open target PST and merge contacts from source PST
            try
            {
                using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
                {
                    // Merge the entire source PST into the target PST.
                    // This will bring over contacts and resolve conflicts by keeping the latest changes.
                    targetPst.MergeWith(new string[] { sourcePstPath });
                    Console.WriteLine("Contacts synchronized successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during synchronization: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
