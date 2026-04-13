using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output MSG file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "VotingFlagMessage.msg");
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage())
            {
                message.Subject = "Sample Message with Voting and Follow‑up";
                message.Body = "Please review the options and respond.";

                // Add a voting button
                FollowUpManager.AddVotingButton(message, "Approve");

                // Set a follow‑up flag
                FollowUpManager.SetFlag(message, "Please follow up");

                // Save the message to MSG file
                try
                {
                    message.Save(outputPath);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ioEx.Message}");
                    return;
                }
            }

            // Load the saved MSG file to verify features
            if (!File.Exists(outputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(outputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("Saved MSG file not found.");
                return;
            }

            using (MapiMessage loadedMessage = MapiMessage.Load(outputPath))
            {
                // Verify voting button
                string[] votingButtons = FollowUpManager.GetVotingButtons(loadedMessage);
                bool hasApproveButton = votingButtons != null && votingButtons.Contains("Approve");

                // Verify follow‑up flag
                FollowUpOptions options = FollowUpManager.GetOptions(loadedMessage);
                bool hasFlagRequest = options != null && !string.IsNullOrEmpty(options.FlagRequest);

                Console.WriteLine($"Voting button 'Approve' present: {hasApproveButton}");
                Console.WriteLine($"Follow‑up flag present: {hasFlagRequest}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
