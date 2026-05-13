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
            // Define file paths
            string sourcePstPath1 = "source1.pst";
            string sourcePstPath2 = "source2.pst";
            string targetPstPath = "merged.pst";

            // Ensure source PST files exist; create minimal placeholders if missing
            EnsurePstExists(sourcePstPath1);
            EnsurePstExists(sourcePstPath2);

            // Ensure target PST exists; create if it does not
            PersonalStorage targetPst;
            if (File.Exists(targetPstPath))
            {
                targetPst = PersonalStorage.FromFile(targetPstPath);
            }
            else
            {
                // Create a new Unicode PST file
                targetPst = PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode);
            }

            // Use the target PST within a using block to guarantee disposal
            using (targetPst)
            {
                // Merge the source PST files into the target PST
                string[] sourceFiles = new string[] { sourcePstPath1, sourcePstPath2 };
                targetPst.MergeWith(sourceFiles);
                Console.WriteLine("PST files merged successfully into '{0}'.", targetPstPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Helper method to ensure a PST file exists; creates an empty PST if absent
    private static void EnsurePstExists(string pstPath)
    {
        try
        {
            if (!File.Exists(pstPath))
            {
                // Create an empty Unicode PST as a placeholder
                using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // No additional content needed for placeholder
                }

                Console.WriteLine("Created placeholder PST: " + pstPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to create placeholder PST '{0}': {1}", pstPath, ex.Message);
            throw;
        }
    }
}
