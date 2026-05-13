using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailThreadPoolExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input directory containing .vcf files
                string inputDirectory = "Contacts";
                // Output directory for converted .msg files
                string outputDirectory = "Converted";

                // Ensure input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                    return;
                }

                // Create output directory if it does not exist
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                // Get all .vcf files in the input directory
                string[] contactFiles;
                try
                {
                    contactFiles = Directory.GetFiles(inputDirectory, "*.vcf");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to enumerate files: {ioEx.Message}");
                    return;
                }

                if (contactFiles.Length == 0)
                {
                    Console.WriteLine("No contact files found to process.");
                    return;
                }

                // Use CountdownEvent to wait for all thread‑pool tasks to finish
                using (CountdownEvent countdown = new CountdownEvent(contactFiles.Length))
                {
                    foreach (string contactPath in contactFiles)
                    {
                        // Queue each conversion work item to the thread pool
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            string inputPath = (string)state;
                            try
                            {
                                // Guard against missing file
                                if (!File.Exists(inputPath))
                                {
                                    Console.Error.WriteLine($"File not found: {inputPath}");
                                    return;
                                }

                                // Load the VCard as a MapiContact
                                using (MapiContact contact = MapiContact.FromVCard(inputPath))
                                {
                                    // Prepare output file path
                                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);
                                    string outputPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".msg");

                                    // Save the contact as a MSG file
                                    contact.Save(outputPath);
                                    Console.WriteLine($"Converted: {inputPath} -> {outputPath}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error processing '{inputPath}': {ex.Message}");
                            }
                            finally
                            {
                                // Signal task completion
                                countdown.Signal();
                            }
                        }, contactPath);
                    }

                    // Wait for all conversions to complete
                    countdown.Wait();
                }

                Console.WriteLine("All contacts have been processed.");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unhandled exception: {e.Message}");
            }
        }
    }
}
