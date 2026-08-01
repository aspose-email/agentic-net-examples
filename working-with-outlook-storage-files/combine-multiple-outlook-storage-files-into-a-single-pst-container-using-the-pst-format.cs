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
            // Author note: This example merges multiple PST files into a single PST container.
            string targetPstPath = "merged.pst";

            // Define source PST files to merge.
            string[] sourcePstPaths = new string[] { "source1.pst", "source2.pst", "source3.pst" };

            // Verify that each source PST file exists.
            foreach (string srcPath in sourcePstPaths)
            {
                if (!File.Exists(srcPath))
                {
                    Console.Error.WriteLine($"Source PST file not found: {srcPath}");
                    return;
                }
            }

            // Ensure the directory for the target PST exists.
            string targetDirectory = Path.GetDirectoryName(targetPstPath);
            if (!string.IsNullOrEmpty(targetDirectory) && !Directory.Exists(targetDirectory))
            {
                Console.Error.WriteLine($"Target directory does not exist: {targetDirectory}");
                return;
            }

            // If a target PST already exists, delete it to create a fresh container.
            if (File.Exists(targetPstPath))
            {
                try
                {
                    File.Delete(targetPstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unable to delete existing target PST: {ex.Message}");
                    return;
                }
            }

            // Create a new PST file with Unicode format.
            using (PersonalStorage pst = PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode))
            {
                // Merge the source PST files into the newly created PST.
                pst.MergeWith(sourcePstPaths);
                Console.WriteLine($"Successfully merged {sourcePstPaths.Length} PST files into '{targetPstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
