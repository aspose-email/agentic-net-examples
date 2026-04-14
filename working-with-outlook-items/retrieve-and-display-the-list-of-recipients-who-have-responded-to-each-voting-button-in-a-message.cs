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
            string messagePath = "votingMessage.msg";

            // Ensure the message file exists; create a minimal placeholder if it does not.
            if (!File.Exists(messagePath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient1@example.com",
                    "Voting Message",
                    "Please select a voting option."))
                {
                    // Add sample voting buttons.
                    FollowUpManager.AddVotingButton(placeholder, "Approve");
                    FollowUpManager.AddVotingButton(placeholder, "Reject");

                    // Save the placeholder message.
                    placeholder.Save(messagePath);
                }
            }

            // Load the message that contains voting buttons.
            using (MapiMessage message = MapiMessage.Load(messagePath))
            {
                // Retrieve voting buttons.
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);
                Console.WriteLine("Voting Buttons:");
                foreach (string button in votingButtons)
                {
                    Console.WriteLine("- " + button);
                }

                // Display recipients and their response status.
                Console.WriteLine("Recipient Responses:");
                foreach (MapiRecipient recipient in message.Recipients)
                {
                    Console.WriteLine(
                        $"Recipient: {recipient.DisplayName} <{recipient.EmailAddress}> " +
                        $"Status: {recipient.RecipientTrackStatus}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
