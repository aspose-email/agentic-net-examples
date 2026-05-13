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
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the body of the email."))
            {
                // Add a voting button named "Approve"
                FollowUpManager.AddVotingButton(message, "Approve");

                // Save the updated message to a file
                message.Save(outputPath);
                Console.WriteLine($"Message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
