using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input contact files (VCF). Adjust paths as needed.
            string[] inputFiles = new string[] { "contact1.vcf", "contact2.vcf", "contact3.vcf" };
            string outputDirectory = "output";

            // Ensure output directory exists.
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Prepare a countdown event to wait for all thread‑pool tasks.
            CountdownEvent countdown = new CountdownEvent(inputFiles.Length);

            foreach (string inputFile in inputFiles)
            {
                // Guard missing input files: create a minimal placeholder if absent.
                if (!File.Exists(inputFile))
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(inputFile, false))
                        {
                            writer.WriteLine("BEGIN:VCARD");
                            writer.WriteLine("VERSION:2.1");
                            writer.WriteLine("FN:Placeholder");
                            writer.WriteLine("END:VCARD");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder for '{inputFile}': {ex.Message}");
                        countdown.Signal();
                        continue;
                    }
                }

                // Queue work to the thread pool.
                ThreadPool.QueueUserWorkItem(state =>
                {
                    try
                    {
                        string filePath = (string)state;

                        // Load the contact from VCF.
                        using (MapiContact contact = MapiContact.FromVCard(filePath))
                        {
                            string outputPath = Path.Combine(outputDirectory,
                                Path.GetFileNameWithoutExtension(filePath) + "_processed.vcf");

                            // Save the contact (could be transformed or simply copied).
                            contact.Save(outputPath);
                            Console.WriteLine($"Processed '{filePath}' -> '{outputPath}'.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing '{state}': {ex.Message}");
                    }
                    finally
                    {
                        // Signal task completion.
                        countdown.Signal();
                    }
                }, inputFile);
            }

            // Wait for all tasks to finish.
            countdown.Wait();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
