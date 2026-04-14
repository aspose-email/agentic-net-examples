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
            // Path to the MSG file
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body"))
                {
                    placeholder.Save(msgPath);
                    Console.WriteLine($"Placeholder MSG created at {msgPath}");
                }
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Retrieve voting buttons
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                Console.WriteLine("Voting Buttons:");
                if (votingButtons != null && votingButtons.Length > 0)
                {
                    foreach (string button in votingButtons)
                    {
                        Console.WriteLine("- " + button);
                    }
                }
                else
                {
                    Console.WriteLine("No voting buttons found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
