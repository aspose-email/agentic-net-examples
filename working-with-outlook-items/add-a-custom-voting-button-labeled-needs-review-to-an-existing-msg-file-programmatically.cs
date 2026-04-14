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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage())
                {
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder message.";
                    placeholder.Save(inputPath);
                }
                Console.Error.WriteLine($"Input file not found. Created placeholder at {inputPath}.");
            }

            // Load the existing MSG file.
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Add a custom voting button labeled "Needs Review".
                FollowUpManager.AddVotingButton(message, "Needs Review");

                // Save the modified message to a new file.
                message.Save(outputPath);
            }

            Console.WriteLine($"Voting button added and saved to {outputPath}.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
