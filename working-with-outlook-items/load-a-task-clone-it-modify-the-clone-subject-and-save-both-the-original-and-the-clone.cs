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
            string originalPath = "originalTask.msg";
            string clonePath = "cloneTask.msg";

            // Ensure the original task file exists; create a minimal placeholder if it does not.
            if (!File.Exists(originalPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage();
                    placeholder.Subject = "Sample Task";
                    placeholder.Body = "This is a placeholder task.";
                    placeholder.MessageClass = "IPM.Task";
                    placeholder.Save(originalPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the original task message.
            using (MapiMessage originalMessage = MapiMessage.Load(originalPath))
            {
                // Clone the original message.
                using (MapiMessage clonedMessage = (MapiMessage)originalMessage.Clone())
                {
                    // Modify the subject of the cloned task.
                    clonedMessage.Subject = "Modified Task Subject";

                    // Save the original message (unchanged or updated).
                    try
                    {
                        originalMessage.Save(originalPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save original task: {ex.Message}");
                    }

                    // Save the cloned message to a new file.
                    try
                    {
                        clonedMessage.Save(clonePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save cloned task: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
