using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output MSG file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                {
                    try
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder MSG created at {inputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                        return;
                    }
                }
            }

            // Load the MSG file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Clear all voting buttons from the message
                FollowUpManager.ClearVotingButtons(message);

                // Verify that no voting buttons remain
                string[] remainingButtons = FollowUpManager.GetVotingButtons(message);
                if (remainingButtons != null && remainingButtons.Length > 0)
                {
                    Console.Error.WriteLine("Voting buttons were not cleared.");
                }
                else
                {
                    Console.WriteLine("Voting buttons cleared successfully.");
                }

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
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

                // Save the modified MSG file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Modified MSG saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
