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
            // Define output path
            string outputPath = "CancelVotingButton.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Meeting Request with Cancel Voting",
                "Please review the meeting details and vote."))
            {
                // Add a "Cancel" voting button
                FollowUpManager.AddVotingButton(message, "Cancel");

                // Save the message as MSG (Unicode format) for Outlook preview
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }

            Console.WriteLine("Message with Cancel voting button saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
