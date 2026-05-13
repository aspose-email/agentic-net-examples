using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPstPath = "input.pst";
                string compressedPstPath = "compressed.pst";

                // Ensure the input PST file exists; create a minimal placeholder if it does not.
                if (!File.Exists(inputPstPath))
                {
                    try
                    {
                        // Create a new Unicode PST file as a placeholder.
                        using (PersonalStorage placeholderPst = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                        {
                            // Placeholder PST created; no additional content required.
                        }
                        Console.WriteLine($"Placeholder PST created at '{inputPstPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                        return;
                    }
                }

                // Open the existing PST and compress it by saving to a new file.
                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
                    {
                        // Save as PST format; this applies built‑in compression.
                        pst.SaveAs(compressedPstPath, FileFormat.Pst);
                    }
                    Console.WriteLine($"Compressed PST saved to '{compressedPstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during PST compression: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
