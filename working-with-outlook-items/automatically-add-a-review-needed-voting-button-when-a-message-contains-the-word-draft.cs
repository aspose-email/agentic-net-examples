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
            string inputPath = "sample.msg";
            string outputPath = "sample_updated.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Sample Subject",
                        "This is a draft message containing the word draft."))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder message created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message, check for the keyword, and add the voting button if needed.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    if (message.Body != null && message.Body.IndexOf("draft", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        FollowUpManager.AddVotingButton(message, "Review Needed");
                        message.Save(outputPath);
                        Console.WriteLine($"Voting button added and message saved to '{outputPath}'.");
                    }
                    else
                    {
                        Console.WriteLine("The message does not contain the word 'draft'. No changes made.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
