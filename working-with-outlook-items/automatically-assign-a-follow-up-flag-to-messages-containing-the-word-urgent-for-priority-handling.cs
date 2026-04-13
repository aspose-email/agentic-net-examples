using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the Outlook message file
            string messagePath = "urgent.msg";

            // Ensure the file exists; if not, create a minimal placeholder
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message."))
                    {
                        placeholder.Save(messagePath);
                    }

                    Console.WriteLine("Placeholder message created at: " + messagePath);
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder message: " + ex.Message);
                    return;
                }
            }

            // Load the existing message
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    bool containsUrgent = false;

                    if (message.Subject != null &&
                        message.Subject.IndexOf("urgent", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        containsUrgent = true;
                    }
                    else if (message.Body != null &&
                             message.Body.IndexOf("urgent", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        containsUrgent = true;
                    }

                    if (containsUrgent)
                    {
                        // Assign a follow‑up flag
                        FollowUpManager.SetFlag(message, "Follow up");

                        // Save the updated message back to the same file
                        message.Save(messagePath);
                        Console.WriteLine("Follow‑up flag assigned to the urgent message.");
                    }
                    else
                    {
                        Console.WriteLine("Message does not contain the word \"urgent\".");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing the message file: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
