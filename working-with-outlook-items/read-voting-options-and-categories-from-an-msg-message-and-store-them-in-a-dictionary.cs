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
            // Define the path to the MSG file.
            string msgPath = "sample.msg";

            // Ensure the directory for the MSG file exists.
            string msgDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(msgDirectory) && !Directory.Exists(msgDirectory))
            {
                Directory.CreateDirectory(msgDirectory);
            }

            // If the MSG file does not exist, create a minimal placeholder.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Message",
                        "This is a placeholder MSG file."))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Retrieve categories and voting buttons.
                IList<string> categoryList = FollowUpManager.GetCategories(message);
                string[] votingButtonArray = FollowUpManager.GetVotingButtons(message);

                // Store the results in a dictionary.
                Dictionary<string, List<string>> votingInfo = new Dictionary<string, List<string>>();
                votingInfo["Categories"] = new List<string>(categoryList);
                votingInfo["VotingButtons"] = new List<string>(votingButtonArray);

                // Output the collected information.
                Console.WriteLine("Categories:");
                foreach (string category in votingInfo["Categories"])
                {
                    Console.WriteLine($"- {category}");
                }

                Console.WriteLine("Voting Buttons:");
                foreach (string button in votingInfo["VotingButtons"])
                {
                    Console.WriteLine($"- {button}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
