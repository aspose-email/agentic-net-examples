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

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder body"))
                {
                    placeholder.Save(inputPath);
                }
            }

            // Load the existing message
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Add voting buttons
                FollowUpManager.AddVotingButton(message, "Approve");
                FollowUpManager.AddVotingButton(message, "Reject");

                // Save the updated message
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
