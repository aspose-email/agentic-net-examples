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
            // Define output file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "VotingMessage.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the MSG message and add voting buttons
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Please review",
                "Kindly select an option: Approve or Reject."))
            {
                // Add voting buttons
                FollowUpManager.AddVotingButton(message, "Approve");
                FollowUpManager.AddVotingButton(message, "Reject");

                // Save the message to file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
