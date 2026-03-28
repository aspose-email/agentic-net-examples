using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define source PST files to be merged
            string[] sourcePstPaths = new string[]
            {
                "source1.pst",
                "source2.pst",
                "source3.pst"
            };

            // Define the target PST file that will contain the merged data
            string targetPstPath = "combined.pst";

            // Verify that each source PST file exists
            foreach (string sourcePath in sourcePstPaths)
            {
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Error: Source PST file not found – {sourcePath}");
                    return;
                }
            }

            // Ensure the target directory exists
            string targetDirectory = Path.GetDirectoryName(targetPstPath);
            if (!string.IsNullOrEmpty(targetDirectory) && !Directory.Exists(targetDirectory))
            {
                try
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {targetDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // If a target file already exists, delete it to allow creation of a fresh PST
            if (File.Exists(targetPstPath))
            {
                try
                {
                    File.Delete(targetPstPath);
                }
                catch (Exception delEx)
                {
                    Console.Error.WriteLine($"Error: Unable to delete existing target PST – {targetPstPath}. {delEx.Message}");
                    return;
                }
            }

            // Create the target PST and merge the source PSTs
            try
            {
                using (PersonalStorage targetPst = PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode))
                {
                    // Merge all source PST files into the target PST
                    targetPst.MergeWith(sourcePstPaths);
                }

                Console.WriteLine("PST files merged successfully into: " + targetPstPath);
            }
            catch (Exception mergeEx)
            {
                Console.Error.WriteLine($"Error during PST merge: {mergeEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
