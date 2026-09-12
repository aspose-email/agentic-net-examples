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
            // Author note: This example merges multiple PST files into a single PST archive,
            // preserving the original folder hierarchy and all messages.

            // Define source PST file paths.
            string[] sourcePstPaths = new string[] { "source1.pst", "source2.pst" };
            // Define the target merged PST file path.
            string targetPstPath = "merged.pst";

            // Ensure each source PST exists; create a minimal placeholder if missing.
            foreach (string srcPath in sourcePstPaths)
            {
                if (!File.Exists(srcPath))
                {
                    // Create an empty PST with Unicode format.
                    using (PersonalStorage.Create(srcPath, FileFormatVersion.Unicode)) { }
                    Console.WriteLine($"Created placeholder source PST: {srcPath}");
                }
            }

            // Ensure the target PST exists; create it if missing.
            if (!File.Exists(targetPstPath))
            {
                using (PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode)) { }
                Console.WriteLine($"Created target PST: {targetPstPath}");
            }

            // Open the target PST and merge the source PSTs.
            using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
            {
                targetPst.MergeWith(sourcePstPaths);
                Console.WriteLine("PST files merged successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
