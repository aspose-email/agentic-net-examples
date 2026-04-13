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
            // Define input MSG file path and output configuration file path.
            string inputMsgPath = "input.msg";
            string outputConfigPath = "voting_buttons.txt";

            // Verify that the input MSG file exists.
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputMsgPath}' does not exist.");
                return;
            }

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(outputConfigPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG message.
            using (MapiMessage message = MapiMessage.Load(inputMsgPath))
            {
                // Retrieve voting buttons.
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                // Write voting buttons to the configuration file.
                using (StreamWriter writer = new StreamWriter(outputConfigPath))
                {
                    foreach (string button in votingButtons)
                    {
                        writer.WriteLine(button);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
