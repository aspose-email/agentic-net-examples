using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string messagePath = "message.msg";

            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("Message file not found: " + messagePath);
                return;
            }

            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    // Retrieve voting buttons
                    string[] votingButtons = FollowUpManager.GetVotingButtons(message);
                    Console.WriteLine("Voting Buttons:");
                    foreach (string button in votingButtons)
                    {
                        Console.WriteLine("- " + button);
                    }

                    // Retrieve the client submit time (UTC)
                    object clientSubmitObj = message.Properties[MapiPropertyTag.PR_CLIENT_SUBMIT_TIME];
                    if (!(clientSubmitObj is DateTime clientSubmitTime))
                    {
                        Console.Error.WriteLine("Client submit time property not found.");
                        return;
                    }

                    // Collect response timestamps from recipients
                    List<DateTime> responseTimes = new List<DateTime>();
                    foreach (MapiRecipient recipient in message.Recipients)
                    {
                        object responseObj = recipient.Properties[MapiPropertyTag.PR_RECIPIENT_TRACKSTATUS_TIME];
                        if (responseObj is DateTime responseTime)
                        {
                            responseTimes.Add(responseTime);
                        }
                    }

                    if (responseTimes.Count == 0)
                    {
                        Console.WriteLine("No voting responses found.");
                        return;
                    }

                    // Calculate average response time in seconds
                    double totalSeconds = 0;
                    foreach (DateTime responseTime in responseTimes)
                    {
                        TimeSpan diff = responseTime - clientSubmitTime;
                        totalSeconds += diff.TotalSeconds;
                    }

                    double averageSeconds = totalSeconds / responseTimes.Count;
                    Console.WriteLine($"Average response time: {averageSeconds:F2} seconds.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing the message: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
