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
            // Path to the message file (placeholder if not present)
            string messagePath = "sample.msg";

            // Ensure the file exists; create a minimal placeholder if missing
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message inside a using block to ensure disposal
            using (MapiMessage message = MapiMessage.Load(messagePath))
            {
                // Check if any flags are set (excluding the zero flag)
                if (message.Flags != MapiMessageFlags.MSGFLAG_ZERO)
                {
                    // Validate that there is at least one recipient in the "To" field
                    if (message.Recipients == null || message.Recipients.Count == 0)
                    {
                        Console.WriteLine("Warning: Message has follow‑up flags but no recipients in the To field.");
                    }
                    else
                    {
                        Console.WriteLine("Message is valid: contains follow‑up flags and at least one recipient.");
                    }
                }
                else
                {
                    Console.WriteLine("Message does not have any follow‑up flags.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
