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

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Subject", "Body"))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the message safely.
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Define the flag request and dates.
                string flagRequest = "Follow up";
                DateTime startDate = DateTime.Now;
                DateTime dueDate = DateTime.Now.AddDays(2); // Example due date.

                // Validate that the due date is not in the past.
                if (dueDate < DateTime.Now)
                {
                    Console.Error.WriteLine("Due date cannot be in the past.");
                    return;
                }

                // Set the follow‑up flag with the validated dates.
                FollowUpManager.SetFlag(message, flagRequest, startDate, dueDate);

                // Save the updated message.
                string updatedPath = "sample_updated.msg";
                message.Save(updatedPath);
                Console.WriteLine($"Message saved with follow‑up flag to '{updatedPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
