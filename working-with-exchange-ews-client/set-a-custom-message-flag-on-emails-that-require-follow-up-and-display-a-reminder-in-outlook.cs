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

            // Ensure the message file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Sample Subject", "Sample body.");
                    placeholder.Save(msgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the existing message.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }

            // Set a follow‑up flag with a reminder.
            string flagRequest = "Follow up";
            DateTime startDate = DateTime.Now;
            DateTime dueDate = DateTime.Now.AddDays(3);
            FollowUpManager.SetFlag(message, flagRequest, startDate, dueDate);

            // Optionally configure additional follow‑up options (e.g., reminder time).
            FollowUpOptions options = new FollowUpOptions(flagRequest, startDate, dueDate);
            options.ReminderTime = DateTime.Now.AddDays(2);
            FollowUpManager.SetOptions(message, options);

            // Save the updated message back to the file.
            try
            {
                message.Save(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save updated message: {ex.Message}");
                return;
            }

            Console.WriteLine($"Flag \"{flagRequest}\" set with reminder on {options.ReminderTime}.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
