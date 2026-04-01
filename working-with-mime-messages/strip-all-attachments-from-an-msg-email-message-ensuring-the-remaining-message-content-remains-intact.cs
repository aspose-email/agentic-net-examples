using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output MSG file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a simple MAPI message with minimal content
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "Sender Name", "sender@example.com"))
                    {
                        placeholder.Save(inputPath);
                    }
                    Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Copy the input file to the output location (overwrite if it already exists)
            try
            {
                File.Copy(inputPath, outputPath, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to copy MSG file: {ex.Message}");
                return;
            }

            // Remove all attachments from the output MSG file
            try
            {
                // This static method removes attachments in-place and returns the removed collection
                MapiAttachmentCollection removed = MapiMessage.RemoveAttachments(outputPath);
                Console.WriteLine($"Removed {removed.Count} attachment(s) from '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to remove attachments: {ex.Message}");
                return;
            }

            // Load the resulting message to verify that content remains intact
            try
            {
                using (MapiMessage result = MapiMessage.Load(outputPath))
                {
                    Console.WriteLine("Message after stripping attachments:");
                    Console.WriteLine($"Subject: {result.Subject}");
                    Console.WriteLine($"Body: {result.Body}");
                    Console.WriteLine($"Attachment count: {result.Attachments.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load resulting MSG: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
