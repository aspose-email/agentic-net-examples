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
            string messagePath = "message.msg";

            // Guard file existence
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

                Console.Error.WriteLine($"File not found: {messagePath}");
                return;
            }

            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    // Retrieve follow‑up options; null means no follow‑up flag
                    FollowUpOptions options = FollowUpManager.GetOptions(message);

                    if (options != null)
                    {
                        // Validate that there is at least one recipient in the To field
                        if (string.IsNullOrWhiteSpace(message.DisplayTo))
                        {
                            Console.WriteLine("Message has a follow‑up flag but no recipients in the To field.");
                        }
                        else
                        {
                            Console.WriteLine("Message has a follow‑up flag and contains To recipients.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Message does not contain a follow‑up flag.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or processing the message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
