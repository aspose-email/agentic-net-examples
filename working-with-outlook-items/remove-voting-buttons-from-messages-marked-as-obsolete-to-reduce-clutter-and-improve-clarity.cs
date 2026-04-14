using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder path for the message file
            const string messagePath = "obsolete.msg";

            // Guard file existence; create a minimal placeholder if missing
            if (!File.Exists(messagePath))
            {
                try
                {
                    // Create a simple MAPI message marked as "Obsolete"
                    using (var placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Obsolete", "This is a placeholder message."))
                    {
                        // Add a sample voting button to demonstrate removal later
                        FollowUpManager.AddVotingButton(placeholder, "Approve");
                        placeholder.Save(messagePath);
                        Console.WriteLine($"Created placeholder message at '{messagePath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the existing message
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(messagePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message '{messagePath}': {ex.Message}");
                return;
            }

            // Ensure the message is disposed after processing
            using (message)
            {
                // Check if the message subject contains "Obsolete"
                if (message.Subject != null && message.Subject.IndexOf("Obsolete", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Remove all voting buttons from the message
                    FollowUpManager.ClearVotingButtons(message);
                    Console.WriteLine("Voting buttons removed from the message.");

                    // Save the updated message back to the same file
                    try
                    {
                        message.Save(messagePath);
                        Console.WriteLine($"Message saved after modification to '{messagePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save updated message: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Message does not contain 'Obsolete' in the subject; no changes made.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
