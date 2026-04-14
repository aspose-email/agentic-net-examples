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
            string messagePath = "sample.msg";

            // Ensure the message file exists; create a minimal placeholder if it does not.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Sample Subject", "Sample Body"))
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

            // Load the existing message, add a custom category, set a follow‑up flag, and save.
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    // Add a custom category.
                    FollowUpManager.AddCategory(message, "MyCustomCategory");

                    // Set a follow‑up flag with a request string.
                    FollowUpManager.SetFlag(message, "Please review");

                    // Save the changes back to the same file.
                    message.Save(messagePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
