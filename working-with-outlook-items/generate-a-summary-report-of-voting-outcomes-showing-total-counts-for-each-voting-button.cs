using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Verify that the message file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MAPI message inside a using block to ensure proper disposal
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Retrieve the list of voting buttons present in the message
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                // Prepare a dictionary to hold counts for each button
                Dictionary<string, int> buttonCounts = new Dictionary<string, int>();

                // Initialize counts (placeholder logic – actual counts would be derived from message data)
                foreach (string button in votingButtons)
                {
                    buttonCounts[button] = 0;
                }

                // Output the summary report
                Console.WriteLine("Voting Outcomes Summary:");
                foreach (KeyValuePair<string, int> entry in buttonCounts)
                {
                    Console.WriteLine($"Button: {entry.Key}, Total Votes: {entry.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
