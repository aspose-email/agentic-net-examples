using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the target PST and source PST files
            string targetPstPath = "combined.pst";
            List<string> sourcePstPathList = new List<string>
            {
                "source1.pst",
                "source2.pst"
            };

            // Verify that each source PST file exists
            foreach (string sourcePath in sourcePstPathList)
            {
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Error: Source PST file not found – {sourcePath}");
                    return;
                }
            }

            // Ensure the directory for the target PST exists
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

            // Open existing target PST or create a new one if it does not exist
            PersonalStorage targetPst;
            if (File.Exists(targetPstPath))
            {
                try
                {
                    targetPst = PersonalStorage.FromFile(targetPstPath, true);
                }
                catch (Exception openEx)
                {
                    Console.Error.WriteLine($"Error: Unable to open target PST – {targetPstPath}. {openEx.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    targetPst = PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create target PST – {targetPstPath}. {createEx.Message}");
                    return;
                }
            }

            // Use the PST within a using block to ensure proper disposal
            using (targetPst)
            {
                try
                {
                    // Merge the source PST files into the target PST
                    targetPst.MergeWith(sourcePstPathList.ToArray());
                    Console.WriteLine("PST files merged successfully.");
                }
                catch (Exception mergeEx)
                {
                    Console.Error.WriteLine($"Error during merge operation: {mergeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
