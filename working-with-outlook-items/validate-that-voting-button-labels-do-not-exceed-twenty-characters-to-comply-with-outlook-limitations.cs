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
            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Subject", "Body"))
            {
                // Define voting button labels
                string[] votingButtons = new string[]
                {
                    "Approve",
                    "Reject",
                    "Needs more information"
                };

                // Add voting buttons only if the label length does not exceed 20 characters
                foreach (string label in votingButtons)
                {
                    if (label.Length <= 20)
                    {
                        FollowUpManager.AddVotingButton(message, label);
                        Console.WriteLine($"Added voting button: {label}");
                    }
                    else
                    {
                        Console.Error.WriteLine($"Voting button label exceeds 20 characters and will be skipped: {label}");
                    }
                }

                // Save the message to a file (guarded file I/O)
                string outputPath = "output.msg";
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (string.IsNullOrEmpty(directory))
                    {
                        directory = Directory.GetCurrentDirectory();
                    }

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
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
