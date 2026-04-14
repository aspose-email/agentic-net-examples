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

            // Ensure the message file exists; create a minimal placeholder if missing.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the existing message.
            using (MapiMessage message = MapiMessage.Load(messagePath))
            {
                // Retrieve current follow‑up options.
                FollowUpOptions currentOptions = FollowUpManager.GetOptions(message);

                if (currentOptions == null)
                {
                    Console.Error.WriteLine("No follow‑up flag is set on the message.");
                    return;
                }

                // Calculate the new due date by extending the existing due date by two days.
                DateTime newDueDate = currentOptions.DueDate.AddDays(2);

                // Update the flag with the same request and start date, but new due date.
                try
                {
                    FollowUpManager.SetFlag(message, currentOptions.FlagRequest, currentOptions.StartDate, newDueDate);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to set follow‑up flag: {ex.Message}");
                    return;
                }

                // Save the updated message back to the file.
                try
                {
                    message.Save(messagePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save updated MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
