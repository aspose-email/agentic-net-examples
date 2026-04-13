using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            string inputDirectory = "NotesDirectory";
            string outputFilePath = "combined.txt";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Prepare a StringBuilder for concatenated bodies
            StringBuilder combinedBody = new StringBuilder();

            // Get all NSF files in the directory
            string[] nsfFiles;
            try
            {
                nsfFiles = Directory.GetFiles(inputDirectory, "*.nsf");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate NSF files: {ex.Message}");
                return;
            }

            foreach (string nsfFile in nsfFiles)
            {
                // Guard against missing file (should not happen after GetFiles, but follow rule)
                if (!File.Exists(nsfFile))
                {
                    Console.Error.WriteLine($"NSF file not found: {nsfFile}");
                    continue;
                }

                // Open the NSF file
                try
                {
                    using (NotesStorageFacility notesFacility = new NotesStorageFacility(nsfFile))
                    {
                        // Enumerate messages (MailMessage objects)
                        foreach (MailMessage message in notesFacility.EnumerateMessages())
                        {
                            if (message != null && !string.IsNullOrEmpty(message.Body))
                            {
                                combinedBody.AppendLine(message.Body);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing NSF file '{nsfFile}': {ex.Message}");
                    // Continue with next file
                }
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Write the combined text to the output file
            try
            {
                File.WriteAllText(outputFilePath, combinedBody.ToString());
                Console.WriteLine($"Combined body written to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write output file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
