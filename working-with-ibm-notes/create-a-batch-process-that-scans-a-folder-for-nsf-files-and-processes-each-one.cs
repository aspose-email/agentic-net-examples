using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            // Define the folder to scan for NSF files
            string folderPath = "NsfFolder";

            // Ensure the folder exists; create it if missing
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{folderPath}': {ex.Message}");
                    return;
                }
            }

            // Get all NSF files in the folder
            string[] nsfFiles;
            try
            {
                nsfFiles = Directory.GetFiles(folderPath, "*.nsf");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing files in '{folderPath}': {ex.Message}");
                return;
            }

            // Process each NSF file
            foreach (string nsfPath in nsfFiles)
            {
                // Guard against missing files; create a minimal placeholder if needed
                if (!File.Exists(nsfPath))
                {
                    try
                    {
                        // Create an empty placeholder NSF file
                        File.WriteAllBytes(nsfPath, new byte[0]);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unable to create placeholder for '{nsfPath}': {ex.Message}");
                        continue;
                    }
                }

                // Open and process the NSF file
                try
                {
                    using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
                    {
                        // Enumerate messages contained in the NSF storage
                        foreach (var message in nsf.EnumerateMessages())
                        {
                            // Output basic information about each message
                            Console.WriteLine($"File: {Path.GetFileName(nsfPath)} | Subject: {message.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing NSF file '{nsfPath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
