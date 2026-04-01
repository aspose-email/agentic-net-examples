using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define source PST files
            string[] sourcePstPaths = new string[] { "source1.pst", "source2.pst", "source3.pst" };
            // Define output PST file
            string outputPstPath = "combined.pst";

            // Verify that each source PST file exists
            foreach (string srcPath in sourcePstPaths)
            {
                if (!File.Exists(srcPath))
                {
                    Console.Error.WriteLine($"Error: Source PST file not found – {srcPath}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Delete existing output PST if it exists
            if (File.Exists(outputPstPath))
            {
                try
                {
                    File.Delete(outputPstPath);
                }
                catch (Exception delEx)
                {
                    Console.Error.WriteLine($"Error: Unable to delete existing output PST – {outputPstPath}. {delEx.Message}");
                    return;
                }
            }

            // Create a new PST file and merge the source PSTs into it
            using (PersonalStorage targetPst = PersonalStorage.Create(outputPstPath, FileFormatVersion.Unicode))
            {
                try
                {
                    targetPst.MergeWith(sourcePstPaths);
                }
                catch (Exception mergeEx)
                {
                    Console.Error.WriteLine($"Error: Merging PST files failed – {mergeEx.Message}");
                    return;
                }
            }

            Console.WriteLine($"Successfully merged PST files into {outputPstPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
