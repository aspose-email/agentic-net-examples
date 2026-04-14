using System;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new MAPI message (use 4‑argument constructor as required)
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test Body"))
            {
                // Verify that initially there are no voting buttons
                string[] initialButtons = FollowUpManager.GetVotingButtons(message);
                Console.WriteLine(initialButtons.Length == 0 ? "Initial state OK" : "Initial state FAILED");

                // Add a voting button
                FollowUpManager.AddVotingButton(message, "Approve");
                string[] afterAdd = FollowUpManager.GetVotingButtons(message);
                bool addCheck = afterAdd.Contains("Approve") && afterAdd.Length == 1;
                Console.WriteLine(addCheck ? "AddVotingButton OK" : "AddVotingButton FAILED");

                // Remove the voting button
                FollowUpManager.RemoveVotingButton(message, "Approve");
                string[] afterRemove = FollowUpManager.GetVotingButtons(message);
                Console.WriteLine(afterRemove.Length == 0 ? "RemoveVotingButton OK" : "RemoveVotingButton FAILED");

                // Add multiple voting buttons
                FollowUpManager.AddVotingButton(message, "Yes");
                FollowUpManager.AddVotingButton(message, "No");
                string[] multipleButtons = FollowUpManager.GetVotingButtons(message);
                bool multipleCheck = multipleButtons.Contains("Yes") && multipleButtons.Contains("No") && multipleButtons.Length == 2;
                Console.WriteLine(multipleCheck ? "Multiple AddVotingButton OK" : "Multiple AddVotingButton FAILED");

                // Clear all voting buttons
                FollowUpManager.ClearVotingButtons(message);
                string[] afterClear = FollowUpManager.GetVotingButtons(message);
                Console.WriteLine(afterClear.Length == 0 ? "ClearVotingButtons OK" : "ClearVotingButtons FAILED");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
