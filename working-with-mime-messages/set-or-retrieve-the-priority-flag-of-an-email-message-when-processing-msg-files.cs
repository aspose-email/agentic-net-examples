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
            string inputPath = "sample.msg";
            string outputPath = "sample_updated.msg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Retrieve current priority (if set)
                object currentPriorityObj = null;
                try
                {
                    currentPriorityObj = message.GetProperty(KnownPropertyList.Priority);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Warning: Unable to read priority – {ex.Message}");
                }

                int currentPriority = currentPriorityObj != null ? (int)currentPriorityObj : -1;
                Console.WriteLine($"Current Priority: {(currentPriority >= 0 ? currentPriority.ToString() : "Not set")}");

                // Set priority to High (value 2)
                int newPriority = 2; // 0 = Low, 1 = Normal, 2 = High (as per MAPI spec)
                message.SetProperty(KnownPropertyList.Priority, newPriority);
                Console.WriteLine($"Priority set to: {newPriority}");

                // Save the updated message
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving file – {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
