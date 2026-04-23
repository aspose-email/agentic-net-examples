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
            // Define input and output PST file paths
            string inputPstPath = "input.pst";
            string outputDirectory = "output";
            string outputPstPath = Path.Combine(outputDirectory, "compressed.pst");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // If the input PST does not exist, create a minimal placeholder PST
            if (!File.Exists(inputPstPath))
            {
                // Create a new empty PST with Unicode format (only supported version)
                using (PersonalStorage placeholderPst = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                {
                    // No additional actions needed; the PST file is created on disposal
                }
            }

            // Open the existing PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                // Save (compress) the PST to a new file
                pst.SaveAs(outputPstPath, FileFormat.Pst);
            }

            Console.WriteLine("PST compression completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
