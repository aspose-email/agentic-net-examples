using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the MSG files to compare
            string msgPath1 = "message1.msg";
            string msgPath2 = "message2.msg";

            // Verify that both files exist before proceeding
            if (!File.Exists(msgPath1))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath1);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {msgPath1}");
                return;
            }

            if (!File.Exists(msgPath2))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath2}");
                return;
            }

            // Load the first message
            using (MapiMessage message1 = MapiMessage.Load(msgPath1))
            {
                // Load the second message
                using (MapiMessage message2 = MapiMessage.Load(msgPath2))
                {
                    // Retrieve voting button configurations
                    string[] buttons1 = FollowUpManager.GetVotingButtons(message1);
                    string[] buttons2 = FollowUpManager.GetVotingButtons(message2);

                    // Use sets for comparison
                    HashSet<string> set1 = new HashSet<string>(buttons1);
                    HashSet<string> set2 = new HashSet<string>(buttons2);

                    // Determine differences
                    List<string> onlyInFirst = set1.Except(set2).ToList();
                    List<string> onlyInSecond = set2.Except(set1).ToList();
                    List<string> common = set1.Intersect(set2).ToList();

                    // Output results
                    Console.WriteLine("Voting buttons present in the first message only:");
                    foreach (string btn in onlyInFirst)
                    {
                        Console.WriteLine($"  {btn}");
                    }

                    Console.WriteLine("Voting buttons present in the second message only:");
                    foreach (string btn in onlyInSecond)
                    {
                        Console.WriteLine($"  {btn}");
                    }

                    Console.WriteLine("Voting buttons common to both messages:");
                    foreach (string btn in common)
                    {
                        Console.WriteLine($"  {btn}");
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
