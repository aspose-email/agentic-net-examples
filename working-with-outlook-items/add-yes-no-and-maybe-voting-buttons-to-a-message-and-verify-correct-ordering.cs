using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Voting Example", "Please select an option."))
            {
                // Add voting buttons in the desired order
                FollowUpManager.AddVotingButton(message, "Yes");
                FollowUpManager.AddVotingButton(message, "No");
                FollowUpManager.AddVotingButton(message, "Maybe");

                // Retrieve voting buttons to verify ordering
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                // Output the voting buttons
                Console.WriteLine("Voting buttons added to the message:");
                foreach (string button in votingButtons)
                {
                    Console.WriteLine(button);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
