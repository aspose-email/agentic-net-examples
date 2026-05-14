using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output_cleaned.msg";
            string tempPath = "temp.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Prepare temporary copy for processing
            try
            {
                File.Copy(inputPath, tempPath, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to copy file to temporary location: {ex.Message}");
                return;
            }

            // Remove all attachments (including embedded images) from the temporary file
            try
            {
                MapiMessage.DestroyAttachments(tempPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to destroy attachments: {ex.Message}");
                return;
            }

            // Load the cleaned message and save as the final output
            try
            {
                using (MapiMessage cleanedMessage = MapiMessage.Load(tempPath))
                {
                    cleanedMessage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or save cleaned message: {ex.Message}");
                return;
            }
            finally
            {
                // Clean up temporary file
                try
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
                catch
                {
                    // Suppress any cleanup errors
                }
            }

            Console.WriteLine($"Cleaned message saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
