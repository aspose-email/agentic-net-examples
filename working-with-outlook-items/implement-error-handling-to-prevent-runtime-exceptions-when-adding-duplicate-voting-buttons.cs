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
            // Output file path for the MSG file
            string outputPath = "output.msg";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body"))
            {
                string votingButtonName = "Approve";

                // Retrieve existing voting buttons
                string[] existingButtons = FollowUpManager.GetVotingButtons(message);
                bool buttonExists = false;
                if (existingButtons != null)
                {
                    foreach (string btn in existingButtons)
                    {
                        if (string.Equals(btn, votingButtonName, StringComparison.OrdinalIgnoreCase))
                        {
                            buttonExists = true;
                            break;
                        }
                    }
                }

                // Add the voting button only if it does not already exist
                if (!buttonExists)
                {
                    FollowUpManager.AddVotingButton(message, votingButtonName);
                    Console.WriteLine($"Voting button \"{votingButtonName}\" added.");
                }
                else
                {
                    Console.WriteLine($"Voting button \"{votingButtonName}\" already exists. Skipping addition.");
                }

                // Save the message to a file with error handling
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to \"{outputPath}\".");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
