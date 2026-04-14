using System;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailVotingButtonDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a new MAPI message
                using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Subject", "Body"))
                {
                    // Desired voting button display name
                    string newButton = "Approve";

                    // Retrieve existing voting buttons
                    string[] existingButtons = FollowUpManager.GetVotingButtons(message);

                    bool duplicateFound = false;
                    if (existingButtons != null)
                    {
                        foreach (string button in existingButtons)
                        {
                            if (string.Equals(button, newButton, StringComparison.OrdinalIgnoreCase))
                            {
                                duplicateFound = true;
                                break;
                            }
                        }
                    }

                    if (duplicateFound)
                    {
                        Console.WriteLine($"Voting button \"{newButton}\" already exists (case‑insensitive). Skipping addition.");
                    }
                    else
                    {
                        // Add the voting button since it does not exist
                        FollowUpManager.AddVotingButton(message, newButton);
                        Console.WriteLine($"Voting button \"{newButton}\" added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
