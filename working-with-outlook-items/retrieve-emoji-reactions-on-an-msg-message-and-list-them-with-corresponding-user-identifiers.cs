using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "message.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder message."))
                {
                    placeholder.Save(msgPath);
                }

                Console.WriteLine($"Placeholder MSG created at {msgPath}");
                return;
            }

            // Load the MSG file.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Retrieve emoji reactions.
                IList<UserReaction> reactions = FollowUpManager.GetReactions(message);

                if (reactions == null || reactions.Count == 0)
                {
                    Console.WriteLine("No reactions found.");
                }
                else
                {
                    Console.WriteLine("Reactions:");
                    foreach (UserReaction reaction in reactions)
                    {
                        // Output the reaction details; ToString provides a readable representation.
                        Console.WriteLine(reaction.ToString());
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
