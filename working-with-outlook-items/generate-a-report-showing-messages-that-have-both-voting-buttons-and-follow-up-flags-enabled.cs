using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare a collection of sample messages.
            List<MapiMessage> messages = new List<MapiMessage>();

            // Message with both voting button and follow‑up flag.
            MapiMessage msg1 = new MapiMessage("from@example.com", "to@example.com", "Subject 1", "Body 1");
            FollowUpManager.AddVotingButton(msg1, "Approve");
            FollowUpManager.SetFlag(msg1, "Action required");
            messages.Add(msg1);

            // Message with only a voting button.
            MapiMessage msg2 = new MapiMessage("from@example.com", "to@example.com", "Subject 2", "Body 2");
            FollowUpManager.AddVotingButton(msg2, "Yes");
            messages.Add(msg2);

            // Message with only a follow‑up flag.
            MapiMessage msg3 = new MapiMessage("from@example.com", "to@example.com", "Subject 3", "Body 3");
            FollowUpManager.SetFlag(msg3, "Review");
            messages.Add(msg3);

            // Message with neither feature.
            MapiMessage msg4 = new MapiMessage("from@example.com", "to@example.com", "Subject 4", "Body 4");
            messages.Add(msg4);

            Console.WriteLine("Messages that have both voting buttons and a follow‑up flag:");
            foreach (MapiMessage message in messages)
            {
                // Retrieve voting buttons.
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);
                // Retrieve follow‑up options.
                FollowUpOptions options = FollowUpManager.GetOptions(message);

                bool hasVoting = votingButtons != null && votingButtons.Length > 0;
                bool hasFlag = options != null && !string.IsNullOrEmpty(options.FlagRequest);

                if (hasVoting && hasFlag)
                {
                    Console.WriteLine($"- Subject: {message.Subject}");
                }
            }

            // Clean up.
            foreach (MapiMessage message in messages)
            {
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
