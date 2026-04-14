using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a minimal MAPI message (4‑argument constructor is required)
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "Sample body"))
            {
                // Retrieve the voting button definitions from the message
                string[] votingButtons = FollowUpManager.GetVotingButtons(message);

                // Output a markdown table documenting each button
                Console.WriteLine("| Button | Purpose |");
                Console.WriteLine("|---|---|");
                foreach (string button in votingButtons)
                {
                    // Purpose column left empty for user to fill as needed
                    Console.WriteLine($"| {button} |  |");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
