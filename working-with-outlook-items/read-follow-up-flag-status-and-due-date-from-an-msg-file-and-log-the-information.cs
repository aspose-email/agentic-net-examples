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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Retrieve follow‑up options.
                FollowUpOptions options = FollowUpManager.GetOptions(msg);

                // Log flag request (indicates whether a follow‑up flag is set) and due date.
                string flagInfo = string.IsNullOrEmpty(options.FlagRequest) ? "No follow‑up flag" : $"Flag request: {options.FlagRequest}";
                string dueInfo = options.DueDate == DateTime.MinValue ? "No due date" : $"Due date: {options.DueDate}";

                Console.WriteLine(flagInfo);
                Console.WriteLine(dueInfo);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
