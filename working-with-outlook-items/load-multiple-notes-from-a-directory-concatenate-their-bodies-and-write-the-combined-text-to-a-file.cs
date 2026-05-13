using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Directory containing NSF note files
            string notesDirectory = "NotesDirectory";
            // Output file for combined bodies
            string outputFile = "CombinedBodies.txt";

            // Verify the input directory exists
            if (!Directory.Exists(notesDirectory))
            {
                Console.Error.WriteLine($"Directory does not exist: {notesDirectory}");
                return;
            }

            StringBuilder combinedBuilder = new StringBuilder();

            // Process each NSF file in the directory
            foreach (string nsfPath in Directory.GetFiles(notesDirectory, "*.nsf"))
            {
                try
                {
                    // Open the NSF file
                    using (NotesStorageFacility notesFacility = new NotesStorageFacility(nsfPath))
                    {
                        // Enumerate all messages (notes) in the NSF
                        foreach (MailMessage message in notesFacility.EnumerateMessages())
                        {
                            using (message)
                            {
                                // Append the body text if present
                                if (!string.IsNullOrEmpty(message.Body))
                                {
                                    combinedBuilder.AppendLine(message.Body);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log errors for this file and continue with the next one
                    Console.Error.WriteLine($"Failed to process '{nsfPath}': {ex.Message}");
                }
            }

            // Write the concatenated bodies to the output file
            try
            {
                File.WriteAllText(outputFile, combinedBuilder.ToString());
                Console.WriteLine($"Combined bodies written to '{outputFile}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write output file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            // Top-level exception handling
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
